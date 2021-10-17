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
        private readonly IDbContextFactory<IdentityDbContext> _contextFactory;
        private readonly InsertBoundary _insertBoundary;
        private readonly InsertClientApp _insertClientApp;
        private readonly InsertRole _insertRole;
        private readonly InsertPermission  _insertPermission;

        public AddBoundaryHandler(IDbContextFactory<IdentityDbContext> contextFactory, InsertBoundary insertBoundary, InsertClientApp insertClientApp, InsertRole insertRole, InsertPermission insertPermission)
        {
            _contextFactory = contextFactory;
            _insertBoundary = insertBoundary;
            _insertClientApp = insertClientApp;
            _insertRole = insertRole;
            _insertPermission = insertPermission;
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
            var roles = await InsertRolesAsync(context, boundary, request.Roles, cancellationToken);
            var permissions = await InsertPermissionsAsync(context, boundary, request.Permissions, cancellationToken);

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

        private async Task<IEnumerable<ClientApp>> InsertClientAppsAsync(IdentityDbContext context, Boundary boundary, IEnumerable<AddBoundaryRequest.ClientAppObj> requestClientApps, CancellationToken cancellationToken)
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

        private async Task<IEnumerable<Role>> InsertRolesAsync(IdentityDbContext context, Boundary boundary, IEnumerable<AddBoundaryRequest.RoleObj> requestRoles, CancellationToken cancellationToken)
        {
            var roles = new List<Role>();

            foreach (var requestRole in requestRoles)
            {
                var role = await _insertRole.ExecuteAsync
                (
                    context,
                    new()
                    {
                        Name = requestRole.Name,
                        LookupKey = requestRole.LookupKey,
                        Boundary = boundary,
                        BoundaryId = boundary.Id
                    }
                );
                roles.Add(role);
            }

            return roles;
        }

        private async Task<IEnumerable<Permission>> InsertPermissionsAsync(IdentityDbContext context, Boundary boundary, IEnumerable<AddBoundaryRequest.PermissionObj> requestPermissions, CancellationToken cancellationToken)
        {
            var permissions = new List<Permission>();

            foreach (var requestPermission in requestPermissions)
            {
                var permission = await _insertPermission.ExecuteAsync
                (
                    context,
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
            }

            return permissions;
        }
    }
}