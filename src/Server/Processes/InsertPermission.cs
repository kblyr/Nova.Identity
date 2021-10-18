using CodeCompanion.EntityFrameworkCore;
using CodeCompanion.Processes;
using Microsoft.EntityFrameworkCore;
using Nova.Identity.Data;
using Nova.Identity.Entities;
using Nova.Identity.Exceptions;

namespace Nova.Identity.Processes
{
    sealed class InsertPermission : IAsyncProcess<Permission, Permission>
    {
        public async Task<Permission> ExecuteAsync(IProcessContext processContext, Permission permission, CancellationToken cancellationToken = default)
        {
            if (processContext is IdentityDbContext context)
            {
                if (await NameExistsAsync(context, permission.Name, permission.Boundary?.Id, cancellationToken))
                    throw new PermissionNameAlreadyExistsException
                    { 
                        Name = permission.Name,
                        Boundary = permission.Boundary is null ? null : new()
                        {
                            Id = permission.Boundary.Id,
                            Name = permission.Boundary.Name
                        }
                    };

                if (permission.LookupKey is not null && await LookupKeyExistsAsync(context, permission.LookupKey, cancellationToken))
                    throw new PermissionLookupKeyAlreadyExistsException { LookupKey = permission.LookupKey };

                context.Permissions.Add(permission, context.CurrentFootprint);
                await context.TrySaveChangesAsync(cancellationToken);
                return permission;
            }

            throw InvalidProcessContextException.Expects<IdentityDbContext>();
        }

        static async Task<bool> NameExistsAsync(IdentityDbContext context, string name, short? boundaryId, CancellationToken cancellationToken) => await context.Permissions
            .Where(permission => permission.Name == name)
            .Where(permission => permission.BoundaryId == boundaryId)
            .AnyAsync(cancellationToken);

        static async Task<bool> LookupKeyExistsAsync(IdentityDbContext context, string lookupKey, CancellationToken cancellationToken) => await context.Permissions
            .IgnoreQueryFilters()
            .Where(permission => permission.LookupKey == lookupKey)
            .AnyAsync(cancellationToken);
    }
}