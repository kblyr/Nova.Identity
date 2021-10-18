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
    sealed class EditClientAppHandler : IRequestHandler<EditClientAppRequest, EditClientAppResponse>
    {
        readonly IDbContextFactory<IdentityDbContext> _contextFactory;
        readonly UpdateClientApp _updateClientApp;

        public EditClientAppHandler(IDbContextFactory<IdentityDbContext> contextFactory, UpdateClientApp updateClientApp)
        {
            _contextFactory = contextFactory;
            _updateClientApp = updateClientApp;
        }

        public async Task<EditClientAppResponse> Handle(EditClientAppRequest request, CancellationToken cancellationToken)
        {
            using var context = _contextFactory.CreateDbContext();
            using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

            var boundary = await context.GetBoundaryAsync(request.BoundaryId ?? 0, cancellationToken);
            var clientApp = await context.GetClientAppAsync(request.Id, cancellationToken) ?? throw new DataRequiredException<ClientApp>();

            if (HasChanges(clientApp, request))
            {
                clientApp = await _updateClientApp.ExecuteAsync
                (
                    context,
                    clientApp with
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

        static bool HasChanges(ClientApp clientApp, EditClientAppRequest request) =>
            clientApp.Name != request.Name ||
            clientApp.LookupKey != request.LookupKey ||
            clientApp.BoundaryId != request.BoundaryId;
    }
}