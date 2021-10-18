using CodeCompanion.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Nova.Identity.Entities;
using Nova.Identity.Exceptions;

namespace Nova.Identity.Data
{
    static class IdentityDbContextExtensions
    {
        public static async Task<Boundary?> GetBoundaryAsync(this IdentityDbContext context, short id, CancellationToken cancellationToken = default)
        {
            if (id == 0) return null;

            return await context.Boundaries.FindByIdAsync(id, cancellationToken) ?? throw new BoundaryNotFoundException { Id = id };
        }

        public static async Task<ClientApp?> GetClientAppAsync(this IdentityDbContext context, short id, CancellationToken cancellationToken = default)
        {
            if (id == 0) return null;

            return await context.ClientApps.FindByIdAsync(id, cancellationToken) ?? throw new ClientAppNotFoundException { Id = id };
        }

        public static async Task<ClientDevice?> GetClientDeviceAsync(this IdentityDbContext context, long id, CancellationToken cancellationToken = default)
        {
            if (id == 0) return null;

            return await context.ClientDevices.FindByIdAsync(id, cancellationToken) ?? throw new ClientDeviceNotFoundException { Id = id };
        }

        public static async Task<Permission?> GetPermissionAsync(this IdentityDbContext context, int id, CancellationToken cancellationToken = default)
        {
            if (id == 0) return null;

            return await context.Permissions.FindByIdAsync(id, cancellationToken) ?? throw new PermissionNotFoundException { Id = id };
        }

        public static async Task<Role?> GetRoleAsync(this IdentityDbContext context, int id, CancellationToken cancellationToken = default)
        {
            if (id == 0) return null;

            return await context.Roles.FindByIdAsync(id, cancellationToken) ?? throw new RoleNotFoundException { Id = id };
        }

        public static async Task<RolePermission?> GetRolePermissionAsync(this IdentityDbContext context, Role role, Permission permission, CancellationToken cancellationToken = default)
        {
            if (role.Id == 0 || permission.Id == 0) return null;

            return await context.RolePermissions
                .Where(rolePermission => rolePermission.RoleId == role.Id)
                .Where(rolePermission => rolePermission.PermissionId == permission.Id)
                .SingleOrDefaultAsync(cancellationToken) ??
                throw new RolePermissionNotFoundException
                {
                    Role = new()
                    {
                        Id = role.Id,
                        Name = role.Name
                    },
                    Permission = new()
                    {
                        Id = permission.Id,
                        Name = permission.Name
                    }
                };
        }

        public static async Task<User?> GetUserAsync(this IdentityDbContext context, int id, CancellationToken cancellationToken = default)
        {
            if (id == 0) return null;

            return await context.Users.FindByIdAsync(id, cancellationToken) ?? throw new UserNotFoundException { Id = id };
        }
    }
}