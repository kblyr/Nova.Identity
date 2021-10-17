using MediatR;
using Microsoft.EntityFrameworkCore;
using Nova.Identity.Data;
using Nova.Identity.Processes;
using Nova.Identity.Requests;
using Nova.Identity.Responses;

namespace Nova.Identity.Handlers
{
    sealed class AddClientAppHandler : IRequestHandler<AddClientAppRequest, AddClientAppResponse>
    {
        private readonly IDbContextFactory<IdentityDbContext> _contextFactory;
        private readonly InsertClientApp _insertClientApp;

        public AddClientAppHandler(IDbContextFactory<IdentityDbContext> contextFactory, InsertClientApp insertClientApp)
        {
            _contextFactory = contextFactory;
            _insertClientApp = insertClientApp;
        }

        public async Task<AddClientAppResponse> Handle(AddClientAppRequest request, CancellationToken cancellationToken)
        {
            using var context = _contextFactory.CreateDbContext();
            using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

            var boundary = await context.GetBoundaryAsync(request.BoundaryId ?? 0, cancellationToken);
            var clientApp = await _insertClientApp.ExecuteAsync
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
                Id = clientApp.Id,
                Name = clientApp.Name,
                LookupKey = clientApp.LookupKey,
                Boundary = boundary is null ? null : new()
                {
                    Id = boundary.Id,
                    Name = boundary.Name
                }
            };
        }
    }
}