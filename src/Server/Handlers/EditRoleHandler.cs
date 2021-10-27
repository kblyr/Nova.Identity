using MediatR;
using Nova.Identity.Requests;
using Nova.Identity.Responses;
using Microsoft.EntityFrameworkCore;
using Nova.Identity.Data;
using Nova.Identity.Processes;
using CodeCompanion.Exceptions;
using Nova.Identity.Entities;
using CodeCompanion.EntityFrameworkCore;
using Nova.Identity.Exceptions;

namespace Nova.Identity.Handlers
{
    sealed class EditRoleHandler : IRequestHandler<EditRoleRequest, EditRoleResponse>
    {
        readonly IDbContextFactory<IdentityDbContext> _contextFactory;
        readonly UpdateRole _updateRole;
        readonly InsertRolePermission _insertRolePermission;

        public EditRoleHandler(IDbContextFactory<IdentityDbContext> contextFactory, UpdateRole updateRole, InsertRolePermission insertRolePermission)
        {
            _contextFactory = contextFactory;
            _updateRole = updateRole;
            _insertRolePermission = insertRolePermission;
        }

        public async Task<EditRoleResponse> Handle(EditRoleRequest request, CancellationToken cancellationToken)
        {
            using var context = _contextFactory.CreateDbContext();
            using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

            var boundary = await context.GetBoundaryAsync(request.BoundaryId ?? 0, cancellationToken);
            var role = await context.GetRoleAsync(request.Id, cancellationToken) ?? throw new DataRequiredException<Role>();

            if (HasChanges(role, request))
            {
                role = await _updateRole.ExecuteAsync
                (
                    context,
                    role with 
                    {
                        Name = request.Name,
                        LookupKey = request.LookupKey,
                        Boundary = boundary,
                        BoundaryId = boundary?.Id
                    },
                    cancellationToken
                );
            }

            var permissions = await InsertRolePermissionsAsync(context, boundary, role, request.PermissionIds, cancellationToken);
            await DeleteRolePermissionsAsync(context, role, request.RemovedPermissionIds, cancellationToken);

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
                Permissions = permissions.Select(permission => new EditRoleResponse.PermissionObj
                {
                    Id = permission.Id,
                    Name = permission.Name
                })
            };
        }

        async Task<IEnumerable<Permission>> InsertRolePermissionsAsync(IdentityDbContext context, Boundary? boundary, Role role, IEnumerable<int> requestPermissionIds, CancellationToken cancellationToken)
        {
            var permissions = new List<Permission>();

            foreach (var requestPermissionId in requestPermissionIds)
            {
                var permission = await context.GetPermissionAsync(requestPermissionId, cancellationToken) ?? throw new DataRequiredException<Permission>();

                if (permission.BoundaryId != boundary?.Id)
                    throw new PermissionNotInBoundaryException
                    {
                        Permission = new()
                        {
                            Id = permission.Id,
                            Name = permission.Name
                        },
                        Boundary = boundary is null ? null : new()
                        {
                            Id = boundary.Id,
                            Name = boundary.Name
                        }
                    };

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

        async Task DeleteRolePermissionsAsync(IdentityDbContext context, Role role, IEnumerable<int> requestRemovedPermissionIds, CancellationToken cancellationToken)
        {
            foreach (var requestRemovedPermissionId in requestRemovedPermissionIds)
            {
                var permission = await context.Permissions.FindByIdAsync(requestRemovedPermissionId, cancellationToken);

                if (permission is null)
                    continue;

                var rolePermission = await context.RolePermissions
                    .Where(rolePermission => rolePermission.RoleId == role.Id)
                    .Where(rolePermission => rolePermission.PermissionId == permission.Id)
                    .SingleOrDefaultAsync(cancellationToken);

                if (rolePermission is null)
                    continue;

                context.RolePermissions.SoftDelete(rolePermission, context.CurrentFootprint);
            }
        } 

        static bool HasChanges(Role role, EditRoleRequest request) => 
            role.Name != request.Name ||
            role.LookupKey != request.LookupKey ||
            role.BoundaryId != request.BoundaryId;
    }
}