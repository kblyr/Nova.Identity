using CodeCompanion.EntityFrameworkCore;
using CodeCompanion.Processes;
using Microsoft.EntityFrameworkCore;
using Nova.Identity.Data;
using Nova.Identity.Entities;
using Nova.Identity.Exceptions;

namespace Nova.Identity.Processes
{
    sealed class InsertRolePermission : IAsyncProcess<RolePermission, RolePermission>
    {
        public async Task<RolePermission> ExecuteAsync(IProcessContext processContext, RolePermission rolePermission, CancellationToken cancellationToken = default)
        {
            if (processContext is IdentityDbContext context)
            {
                if (await ExistsAsync(context, rolePermission.Role.Id, rolePermission.Permission.Id, cancellationToken))
                    throw new RolePermissionAlreadyExistsException
                    {
                        Role = new()
                        {
                            Id = rolePermission.Role.Id,
                            Name = rolePermission.Role.Name
                        },
                        Permission = new()
                        {
                            Id = rolePermission.Permission.Id,
                            Name = rolePermission.Permission.Name
                        }
                    };

                context.RolePermissions.Add(rolePermission, context.CurrentFootprint);
                await context.TrySaveChangesAsync(cancellationToken);
                return rolePermission;
            }

            throw InvalidProcessContextException.Expects<IdentityDbContext>();
        }

        static async Task<bool> ExistsAsync(IdentityDbContext context, int roleId, int permissionId, CancellationToken cancellationToken) => await context.RolePermissions
            .Where(rolePermission => rolePermission.RoleId == roleId)
            .Where(rolePermission => rolePermission.PermissionId == permissionId)
            .AnyAsync(cancellationToken);
    }
}