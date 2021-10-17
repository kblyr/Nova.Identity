using MediatR;
using Microsoft.EntityFrameworkCore;
using Nova.Identity.Data;
using Nova.Identity.Processes;
using Nova.Identity.Requests;
using Nova.Identity.Responses;

namespace Nova.Identity.Handlers
{
    sealed class AddPermissionHandler : IRequestHandler<AddPermissionRequest, AddPermissionResponse>
    {
        private readonly IDbContextFactory<IdentityDbContext> _contextFactory;
        private readonly InsertPermission _insertPermission;

        public AddPermissionHandler(IDbContextFactory<IdentityDbContext> contextFactory, InsertPermission insertPermission)
        {
            _contextFactory = contextFactory;
            _insertPermission = insertPermission;
        }

        public async Task<AddPermissionResponse> Handle(AddPermissionRequest request, CancellationToken cancellationToken)
        {
            using var context = _contextFactory.CreateDbContext();
            using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

            var boundary = await context.GetBoundaryAsync(request.BoundaryId ?? 0, cancellationToken);
            var permission = await _insertPermission.ExecuteAsync
            (
                context,
                new()
                {
                    Name = request.Name,
                    LookupKey = request.LookupKey,
                    Boundary = boundary,
                    BoundaryId = boundary?.Id
                },
                cancellationToken
            );

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
    }
}