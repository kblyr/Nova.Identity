using CodeCompanion.EntityFrameworkCore;
using CodeCompanion.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nova.Identity.Data;
using Nova.Identity.Entities;
using Nova.Identity.Processes;
using Nova.Identity.Requests;
using Nova.Identity.Responses;

namespace Nova.Identity.Handlers
{
    sealed class AddRoleHandler : IRequestHandler<AddRoleRequest, AddRoleResponse>
    {
        readonly IDbContextFactory<IdentityDbContext> _contextFactory;
        readonly InsertRole _insertRole;
        readonly InsertRolePermission _insertRolePermission;

        public AddRoleHandler(IDbContextFactory<IdentityDbContext> contextFactory, InsertRole insertRole, InsertRolePermission insertRolePermission)
        {
            _contextFactory = contextFactory;
            _insertRole = insertRole;
            _insertRolePermission = insertRolePermission;
        }

        public async Task<AddRoleResponse> Handle(AddRoleRequest request, CancellationToken cancellationToken)
        {
            using var context = _contextFactory.CreateDbContext();
            using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

            var boundary = await context.GetBoundaryAsync(request.BoundaryId ?? 0, cancellationToken);
            var role = await _insertRole.ExecuteAsync
            (
                context.WithHotSave(),
                new()
                {
                    Name = request.Name,
                    LookupKey = request.LookupKey
                },
                cancellationToken
            );
            var permissions = await InsertRolePermissionsAsync(context, role, request.PermissionIds, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return new()
            {
                Id = role.Id,
                Name = role.Name,
                LookupKey = role.LookupKey,
                Boundary = boundary is null ? null : new()
                {
                    Id = boundary.Id,
                    Name = boundary.Name
                },
                Permissions = permissions.Select(permission => new AddRoleResponse.PermissionObj
                {
                    Id = permission.Id,
                    Name = permission.Name
                })
            };
        }

        async Task<IEnumerable<Permission>> InsertRolePermissionsAsync(IdentityDbContext context, Role role, IEnumerable<int> requestPermissionIds, CancellationToken cancellationToken)
        {
            var permissions = new List<Permission>();

            foreach (var requestPermissionId in requestPermissionIds)
            {
                var permission = await context.GetPermissionAsync(requestPermissionId, cancellationToken) ?? throw new DataRequiredException<Permission>();

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

                permissions.Add(permission);
            }

            return permissions;
        }
    }
}