using CodeCompanion.EntityFrameworkCore;
using CodeCompanion.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nova.Identity.Data;
using Nova.Identity.Entities;
using Nova.Identity.Exceptions;
using Nova.Identity.Processes;
using Nova.Identity.Requests;
using Nova.Identity.Responses;

namespace Nova.Identity.Handlers
{
    sealed class EditBoundaryHandler : IRequestHandler<EditBoundaryRequest, EditBoundaryResponse>
    {
        readonly IDbContextFactory<IdentityDbContext> _contextFactory;
        readonly UpdateBoundary _updateBoundary;
        readonly InsertClientApp _insertClientApp;
        readonly InsertRole _insertRole;
        readonly InsertPermission _insertPermission;
        readonly InsertRolePermission _insertRolePermission;
        readonly UpdateClientApp _updateClientApp;
        readonly UpdateRole _updateRole;
        readonly UpdatePermission _updatePermission;

        public EditBoundaryHandler(IDbContextFactory<IdentityDbContext> contextFactory, UpdateBoundary updateBoundary, InsertClientApp insertClientApp, InsertRole insertRole, InsertPermission insertPermission, InsertRolePermission insertRolePermission, UpdateClientApp updateClientApp, UpdateRole updateRole, UpdatePermission updatePermission)
        {
            _contextFactory = contextFactory;
            _updateBoundary = updateBoundary;
            _insertClientApp = insertClientApp;
            _insertRole = insertRole;
            _insertPermission = insertPermission;
            _insertRolePermission = insertRolePermission;
            _updateClientApp = updateClientApp;
            _updateRole = updateRole;
            _updatePermission = updatePermission;
        }

        public async Task<EditBoundaryResponse> Handle(EditBoundaryRequest request, CancellationToken cancellationToken)
        {
            using var context = _contextFactory.CreateDbContext();
            using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

            var boundary = await context.GetBoundaryAsync(request.Id, cancellationToken) ?? throw new DataRequiredException<Boundary>();

            if (HasChanges(boundary, request))
            {
                boundary = await _updateBoundary.ExecuteAsync
                (
                    context,
                    boundary with 
                    {
                        Name = request.Name,
                        LookupKey = request.LookupKey
                    },
                    cancellationToken
                );
            }

            var clientApps = await SaveClientAppsAsync(context, boundary, request.ClientApps, cancellationToken);
            var (roles, rolesWithPermissions) = await SaveRolesAsync(context, boundary, request.Roles, cancellationToken);
            var (permissions, tempPermissions) = await SavePermissionsAsync(context, boundary, request.Permissions, cancellationToken);
            await DeleteClientAppsAsync(context, boundary, request.DeletedClientAppIds, cancellationToken);
            await DeleteRolesAsync(context, boundary, request.DeletedRoleIds, cancellationToken);
            await DeletePermissionsAsync(context, boundary, request.DeletedPermissionIds, cancellationToken);
            await ProcessRoleWithPermissionsAsync(context, rolesWithPermissions, tempPermissions, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return new()
            {
                Id = boundary.Id,
                Name = boundary.Name,
                LookupKey = boundary.LookupKey,
                ClientApps = clientApps.Select(clientApp => new EditBoundaryResponse.ClientAppObj
                {
                    Id = clientApp.Id,
                    Name = clientApp.Name,
                    LookupKey = clientApp.LookupKey
                }),
                Roles = roles.Select(role => new EditBoundaryResponse.RoleObj
                {
                    Id = role.Id,
                    Name = role.Name,
                    LookupKey = role.LookupKey
                }),
                Permissions = permissions.Select(permission => new EditBoundaryResponse.PermissionObj
                {
                    Id = permission.Id,
                    Name = permission.Name,
                    LookupKey = permission.LookupKey
                })
            };
        }

        async Task<IEnumerable<ClientApp>> SaveClientAppsAsync(IdentityDbContext context, Boundary boundary, IEnumerable<EditBoundaryRequest.ClientAppObj> requestClientApps, CancellationToken cancellationToken)
        {
            var clientApps = new List<ClientApp>();

            foreach (var requestClientApp in requestClientApps)
            {
                var clientApp = 
                    await context.GetClientAppAsync(requestClientApp.Id ?? 0, cancellationToken) ??
                    new()
                    {
                        Name = requestClientApp.Name,
                        LookupKey = requestClientApp.LookupKey,
                        Boundary = boundary,
                        BoundaryId = boundary.Id
                    };

                if (clientApp.Id == 0)
                {
                    clientApp = await _insertClientApp.ExecuteAsync
                    (
                        context,
                        clientApp,
                        cancellationToken
                    );
                }
                else if (clientApp.BoundaryId != boundary.Id)
                {
                    throw new ClientAppNotInBoundaryException
                    {
                        ClientApp = new()
                        {
                            Id = clientApp.Id,
                            Name = clientApp.Name
                        },
                        Boundary = new()
                        {
                            Id = boundary.Id,
                            Name = boundary.Name
                        }
                    };
                }
                else if (HasChanges(clientApp, requestClientApp))
                {
                    clientApp = await _updateClientApp.ExecuteAsync
                    (
                        context,
                        clientApp with
                        {
                            Name = requestClientApp.Name,
                            LookupKey = requestClientApp.LookupKey,
                            Boundary = boundary,
                            BoundaryId = boundary.Id
                        },
                        cancellationToken
                    );
                }

                clientApps.Add(clientApp);                
            }

            return clientApps;
        }

        async Task<(IEnumerable<Role> Roles, IEnumerable<(Role Role, IEnumerable<int> PermissionIds, IEnumerable<int> PermissionTempIds, IEnumerable<int> RemovedPermissionIds)> RolesWithPermissions)> SaveRolesAsync(IdentityDbContext context, Boundary boundary, IEnumerable<EditBoundaryRequest.RoleObj> requestRoles, CancellationToken cancellationToken)
        {
            var roles = new List<Role>();
            var rolesWithPermissions = new List<(Role, IEnumerable<int>, IEnumerable<int>, IEnumerable<int>)>();

            foreach (var requestRole in requestRoles)
            {
                var role =
                    await context.GetRoleAsync(requestRole.Id ?? 0, cancellationToken) ??
                    new()
                    {
                        Name = requestRole.Name,
                        LookupKey = requestRole.LookupKey,
                        Boundary = boundary,
                        BoundaryId = boundary.Id
                    };

                if (role.Id == 0)
                {
                    role = await _insertRole.ExecuteAsync
                    (
                        context.WithHotSave(),
                        role,
                        cancellationToken
                    );
                }
                else if (role.BoundaryId != boundary.Id)
                {
                    throw new RoleNotInBoundaryException
                    {
                        Role = new()
                        {
                            Id = role.Id,
                            Name = role.Name
                        },
                        Boundary = new()
                        {
                            Id = boundary.Id,
                            Name = boundary.Name
                        }
                    };
                }
                else if (HasChanges(role, requestRole))
                {
                    role = await _updateRole.ExecuteAsync
                    (
                        context,
                        role with
                        {
                            Name = requestRole.Name,
                            LookupKey = requestRole.LookupKey,
                            Boundary = boundary,
                            BoundaryId = boundary.Id
                        },
                        cancellationToken
                    );
                }

                roles.Add(role);
                rolesWithPermissions.Add((role, requestRole.PermissionIds, requestRole.PermissionTempIds, requestRole.RemovedPermissionIds));
            }

            return (roles, rolesWithPermissions);
        }

        async Task<(IEnumerable<Permission> permissions, IDictionary<int, Permission> TempPermissions)> SavePermissionsAsync(IdentityDbContext context, Boundary boundary, IEnumerable<EditBoundaryRequest.PermissionObj> requestPermissions, CancellationToken cancellationToken)
        {
            var permissions = new List<Permission>();
            var tempPermissions = new Dictionary<int, Permission>();

            foreach (var requestPermission in requestPermissions)
            {
                var permission =
                    await context.GetPermissionAsync(requestPermission.Id ?? 0, cancellationToken) ??
                    new()
                    {
                        Name = requestPermission.Name,
                        LookupKey = requestPermission.LookupKey,
                        Boundary = boundary,
                        BoundaryId = boundary.Id
                    };

                if (permission.Id == 0)
                {
                    permission = await _insertPermission.ExecuteAsync
                    (
                        context.WithHotSave(),
                        permission,
                        cancellationToken
                    );
                }
                else if (permission.BoundaryId != boundary.Id)
                {
                    throw new PermissionNotInBoundaryException
                    {
                        Permission = new()
                        {
                            Id = permission.Id,
                            Name = permission.Name
                        },
                        Boundary = new()
                        {
                            Id = boundary.Id,
                            Name = boundary.Name
                        }
                    };
                }
                else if (HasChanges(permission, requestPermission))
                {
                    permission = await _updatePermission.ExecuteAsync
                    (
                        context,
                        permission with
                        {
                            Name = requestPermission.Name,
                            LookupKey = requestPermission.LookupKey,
                            Boundary = boundary,
                            BoundaryId = boundary.Id
                        },
                        cancellationToken
                    );
                }

                permissions.Add(permission);
                tempPermissions.Add(requestPermission.TempId, permission);
            }

            return (permissions, tempPermissions);
        }

        async Task ProcessRoleWithPermissionsAsync(IdentityDbContext context, IEnumerable<(Role Role, IEnumerable<int> PermissionIds, IEnumerable<int> PermissionTempIds, IEnumerable<int> RemovedPermissionIds)> rolesWithPermissions, IDictionary<int, Permission> tempPermissions, CancellationToken cancellationToken)
        {
            foreach (var roleWithPermissions in rolesWithPermissions)
            {
                var role = roleWithPermissions.Role;

                foreach (var permissionId in roleWithPermissions.PermissionIds)
                {
                    var permission = await context.Permissions.FindByIdAsync(permissionId, cancellationToken);

                    if (permission is null) continue;

                    await _insertRolePermission.ExecuteAsync
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

                foreach (var permissionTempId in roleWithPermissions.PermissionTempIds)
                {
                    var permission = tempPermissions[permissionTempId];

                    await _insertRolePermission.ExecuteAsync
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

                foreach (var removedPermissionId in roleWithPermissions.RemovedPermissionIds)
                {
                    var permission = await context.Permissions.FindByIdAsync(removedPermissionId, cancellationToken);

                    if (permission is null) continue;

                    var rolePermission = await context.RolePermissions
                        .Where(rolePermission => rolePermission.RoleId == role.Id)
                        .Where(rolePermission => rolePermission.PermissionId == permission.Id)
                        .SingleOrDefaultAsync(cancellationToken);

                    if (rolePermission is null) continue;

                    context.RolePermissions.SoftDelete(rolePermission, context.CurrentFootprint);
                }
            }
        }

        async Task DeleteClientAppsAsync(IdentityDbContext context, Boundary boundary, IEnumerable<short> requestDeletedClientAppIds, CancellationToken cancellationToken)
        {
            foreach (var requestDeletedClientAppId in requestDeletedClientAppIds)
            {
                var clientApp = await context.ClientApps.FindByIdAsync(requestDeletedClientAppId, cancellationToken);

                if (clientApp is null) continue;

                if (clientApp.BoundaryId != boundary.Id)
                    throw new ClientAppNotInBoundaryException
                    {
                        ClientApp = new()
                        {
                            Id = clientApp.Id,
                            Name = clientApp.Name
                        },
                        Boundary = new()
                        {
                            Id = boundary.Id,
                            Name = boundary.Name
                        }
                    };

                context.ClientApps.SoftDelete(clientApp, context.CurrentFootprint);
            }
        }

        async Task DeleteRolesAsync(IdentityDbContext context, Boundary boundary, IEnumerable<int> requestDeletedRoleIds, CancellationToken cancellationToken)
        {
            foreach (var requestDeletedRoleId in requestDeletedRoleIds)
            {
                var role = await context.Roles.FindByIdAsync(requestDeletedRoleId, cancellationToken);

                if (role is null) continue;

                if (role.BoundaryId != boundary.Id)
                    throw new RoleNotInBoundaryException
                    {
                        Role = new()
                        {
                            Id = role.Id,
                            Name = role.Name
                        },
                        Boundary = new()
                        {
                            Id = boundary.Id,
                            Name = boundary.Name
                        }
                    };

                context.Roles.SoftDelete(role, context.CurrentFootprint);
            }
        }

        async Task DeletePermissionsAsync(IdentityDbContext context, Boundary boundary, IEnumerable<int> requestDeletedPermissionIds, CancellationToken cancellationToken)
        {
            foreach (var requestDeletedPermissionId in requestDeletedPermissionIds)
            {
                var permission = await context.Permissions.FindByIdAsync(requestDeletedPermissionId, cancellationToken);

                if (permission is null) continue;

                if (permission.BoundaryId != boundary.Id)
                    throw new PermissionNotInBoundaryException
                    {
                        Permission = new()
                        {
                            Id = permission.Id,
                            Name = permission.Name
                        },
                        Boundary = new()
                        {
                            Id = boundary.Id,
                            Name = boundary.Name
                        }
                    };

                context.Permissions.SoftDelete(permission, context.CurrentFootprint);
            }
        }

        static bool HasChanges(Boundary boundary, EditBoundaryRequest request) =>
            boundary.Name != request.Name ||
            boundary.LookupKey != request.LookupKey;

        static bool HasChanges(ClientApp clientApp, EditBoundaryRequest.ClientAppObj requestClientApp) =>
            clientApp.Name != requestClientApp.Name ||
            clientApp.LookupKey != requestClientApp.LookupKey;

        static bool HasChanges(Role role, EditBoundaryRequest.RoleObj requestRole) =>
            role.Name != requestRole.Name ||
            role.LookupKey != requestRole.LookupKey;

        static bool HasChanges(Permission permission, EditBoundaryRequest.PermissionObj requestPermission) =>
            permission.Name != requestPermission.Name ||
            permission.LookupKey != requestPermission.LookupKey;
    }
}