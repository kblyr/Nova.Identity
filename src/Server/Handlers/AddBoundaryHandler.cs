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

        public AddBoundaryHandler(IDbContextFactory<IdentityDbContext> contextFactory, InsertBoundary insertBoundary, InsertClientApp insertClientApp)
        {
            _contextFactory = contextFactory;
            _insertBoundary = insertBoundary;
            _insertClientApp = insertClientApp;
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
            }

            return clientApps;
        }
    }
}