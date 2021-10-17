using CodeCompanion.EntityFrameworkCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nova.Identity.Data;
using Nova.Identity.Entities;
using Nova.Identity.Processes;
using Nova.Identity.Requests;
using Nova.Identity.Responses;

namespace Nova.Identity.Handlers
{
    sealed class AddBoundaryHandler : IRequestHandler<AddBoundaryRequest, AddBoundaryResponse>
    {
        readonly IDbContextFactory<IdentityDbContext> _contextFactory;
        readonly InsertBoundary _insertBoundary;
        readonly InsertClientApp _insertClientApp;
        readonly InsertRole _insertRole;
        readonly InsertPermission  _insertPermission;
        readonly InsertRolePermission _insertRolePermission;

        public AddBoundaryHandler(IDbContextFactory<IdentityDbContext> contextFactory, InsertBoundary insertBoundary, InsertClientApp insertClientApp, InsertRole insertRole, InsertPermission insertPermission, InsertRolePermission insertRolePermission)
        {
            _contextFactory = contextFactory;
            _insertBoundary = insertBoundary;
            _insertClientApp = insertClientApp;
            _insertRole = insertRole;
            _insertPermission = insertPermission;
            _insertRolePermission = insertRolePermission;
        }

        public async Task<AddBoundaryResponse> Handle(AddBoundaryRequest request, CancellationToken cancellationToken)
        {
            using var context = _contextFactory.CreateDbContext();
            using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

            var boundary = await _insertBoundary.ExecuteAsync
            (
                context.WithHotSave(),
                new()
                {
                    Name = request.Name,
                    LookupKey = request.LookupKey
                },
                cancellationToken
            );
            var clientApps = await InsertClientAppsAsync(context, boundary, request.ClientApps, cancellationToken);
            var (roles, rolesWithPermissionTempIds) = await InsertRolesAsync(context, boundary, request.Roles, cancellationToken);
            var (permissions, tempPermissions) = await InsertPermissionsAsync(context, boundary, request.Permissions, cancellationToken);
            await InsertRolePermissionsAsync(context, rolesWithPermissionTempIds, tempPermissions, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return new()
            {
                Id = boundary.Id,
                Name = boundary.Name,
                LookupKey = boundary.LookupKey,
                ClientApps = clientApps.Select(clientApp => new AddBoundaryResponse.ClientAppObj
                {
                    Id = clientApp.Id,
                    Name = clientApp.Name,
                    LookupKey = clientApp.LookupKey  
                }),
                Roles = roles.Select(role => new AddBoundaryResponse.RoleObj
                {
                    Id = role.Id,
                    Name = role.Name,
                    LookupKey = role.LookupKey
                }),
                Permissions = permissions.Select(permission => new AddBoundaryResponse.PermissionObj
                {
                    Id = permission.Id,
                    Name = permission.Name,
                    LookupKey = permission.LookupKey
                })
            };
        }

        async Task<IEnumerable<ClientApp>> InsertClientAppsAsync(IdentityDbContext context, Boundary boundary, IEnumerable<AddBoundaryRequest.ClientAppObj> requestClientApps, CancellationToken cancellationToken)
        {
            var clientApps = new List<ClientApp>();

            foreach (var requestClientApp in requestClientApps)
            {
                var clientApp = await _insertClientApp.ExecuteAsync
                (
                    context,
                    new()
                    {
                        Name = requestClientApp.Name,
                        LookupKey = requestClientApp.LookupKey,
                        Boundary = boundary,
                        BoundaryId = boundary.Id
                    },
                    cancellationToken
                );
                clientApps.Add(clientApp);
            }

            return clientApps;
        }

        async Task<(IEnumerable<Role> Roles, IDictionary<Role, IEnumerable<int>> RolesWithPermissionTempIds)> InsertRolesAsync(IdentityDbContext context, Boundary boundary, IEnumerable<AddBoundaryRequest.RoleObj> requestRoles, CancellationToken cancellationToken)
        {
            var roles = new List<Role>();
            var rolesWithPermissionTempIds = new Dictionary<Role, IEnumerable<int>>();

            foreach (var requestRole in requestRoles)
            {
                var role = await _insertRole.ExecuteAsync
                (
                    context.WithHotSave(),
                    new()
                    {
                        Name = requestRole.Name,
                        LookupKey = requestRole.LookupKey,
                        Boundary = boundary,
                        BoundaryId = boundary.Id
                    }
                );
                roles.Add(role);
                rolesWithPermissionTempIds.Add(role, requestRole.PermissionTempIds);
            }

            return (roles, rolesWithPermissionTempIds);
        }

        async Task<(IEnumerable<Permission> Permissions, IDictionary<int, Permission> TempPermissions)> InsertPermissionsAsync(IdentityDbContext context, Boundary boundary, IEnumerable<AddBoundaryRequest.PermissionObj> requestPermissions, CancellationToken cancellationToken)
        {
            var permissions = new List<Permission>();
            var tempPermissions = new Dictionary<int, Permission>(); 

            foreach (var requestPermission in requestPermissions)
            {
                var permission = await _insertPermission.ExecuteAsync
                (
                    context.WithHotSave(),
                    new()
                    {
                        Name = requestPermission.Name,
                        LookupKey = requestPermission.LookupKey,
                        Boundary = boundary,
                        BoundaryId = boundary.Id
                    },
                    cancellationToken
                );
                permissions.Add(permission);
                tempPermissions.Add(requestPermission.TempId, permission);
            }

            return (permissions, tempPermissions);
        }

        async Task InsertRolePermissionsAsync(IdentityDbContext context, IDictionary<Role, IEnumerable<int>> rolesWithPermissionTempIds, IDictionary<int, Permission> tempPermissions, CancellationToken cancellationToken)
        {
            foreach (var roleWithPermissionTempIds in rolesWithPermissionTempIds)
            {
                var role = roleWithPermissionTempIds.Key;
                
                foreach (var permissionTempId in roleWithPermissionTempIds.Value)
                {
                    var permission = tempPermissions[permissionTempId];
                    var rolePermission = await _insertRolePermission.ExecuteAsync
                    (
                        context,
                        new()
                        {
                            Role = role,
                            RoleId = role.Id,
                            Permission = permission,
                            PermissionId = permission.Id
                        },
                        cancellationToken
                    );
                }
            }
        }
    }
}