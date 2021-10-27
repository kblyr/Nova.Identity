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
    sealed class EditPermissionHandler : IRequestHandler<EditPermissionRequest, EditPermissionResponse>
    {
        readonly IDbContextFactory<IdentityDbContext> _contextFactory;
        readonly UpdatePermission _updatePermission;

        public EditPermissionHandler(IDbContextFactory<IdentityDbContext> contextFactory, UpdatePermission updatePermission)
        {
            _contextFactory = contextFactory;
            _updatePermission = updatePermission;
        }

        public async Task<EditPermissionResponse> Handle(EditPermissionRequest request, CancellationToken cancellationToken)
        {
            using var context = _contextFactory.CreateDbContext();
            using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

            var boundary = await context.GetBoundaryAsync(request.BoundaryId ?? 0, cancellationToken);
            var permission = await context.GetPermissionAsync(request.Id, cancellationToken) ?? throw new DataRequiredException<Permission>();

            if (HasChanges(permission, request))
            {
                permission = await _updatePermission.ExecuteAsync
                (
                    context,
                    permission with 
                    {
                        Name = request.Name,
                        LookupKey = request.LookupKey,
                        Boundary = boundary,
                        BoundaryId = boundary?.Id
                    },
                    cancellationToken
                );
            }

            await context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return new()
            {
                Id = permission.Id,
                Name = permission.Name,
                LookupKey = permission.LookupKey,
                Boundary = boundary is null ? null : new()
                {
                    Id = boundary.Id,
                    Name = boundary.Name
                }
            };
        }

        static bool HasChanges(Permission permission, EditPermissionRequest request) => 
            permission.Name != request.Name ||
            permission.LookupKey != request.LookupKey ||
            permission.BoundaryId != request.BoundaryId;
    }
}