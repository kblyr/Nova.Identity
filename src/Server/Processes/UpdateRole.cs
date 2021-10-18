using CodeCompanion.EntityFrameworkCore;
using CodeCompanion.Processes;
using Microsoft.EntityFrameworkCore;
using Nova.Identity.Data;
using Nova.Identity.Entities;
using Nova.Identity.Exceptions;

namespace Nova.Identity.Processes
{
    sealed class UpdateRole : IAsyncProcess<Role, Role>
    {
        public async Task<Role> ExecuteAsync(IProcessContext processContext, Role role, CancellationToken cancellationToken = default)
        {
            if (processContext is IdentityDbContext context)
            {
                if (await NameExistsAsync(context, role.Id, role.Name, role.Boundary?.Id, cancellationToken))
                    throw new RoleNameAlreadyExistsException
                    {
                        Name = role.Name,
                        Boundary = role.Boundary is null ? null : new()
                        {
                            Id = role.Boundary.Id,
                            Name = role.Boundary.Name
                        }
                    };

                if (role.LookupKey is not null && await LookupKeyExistsAsync(context, role.Id, role.LookupKey, cancellationToken))
                    throw new RoleLookupKeyAlreadyExistsException { LookupKey = role.LookupKey };

                context.Roles.Update(role, context.CurrentFootprint);
                return role;
            }

            throw InvalidProcessContextException.Expects<IdentityDbContext>();
        }

        static async Task<bool> NameExistsAsync(IdentityDbContext context, int id, string name, short? boundaryId, CancellationToken cancellationToken) => await context.Roles
            .Where(role => role.Id != id)
            .Where(role => role.Name == name)
            .Where(role => role.BoundaryId == boundaryId)
            .AnyAsync(cancellationToken);

        static async Task<bool> LookupKeyExistsAsync(IdentityDbContext context, int id, string lookupKey, CancellationToken cancellationToken) => await context.Roles
            .IgnoreQueryFilters()
            .Where(role => role.Id != id)
            .Where(role => role.LookupKey == lookupKey)
            .AnyAsync(cancellationToken);
    }
}