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

            await context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return new()
            {
                Id = boundary.Id,
                Name = boundary.Name,
                LookupKey = boundary.LookupKey
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

        static bool HasChanges(Boundary boundary, EditBoundaryRequest request) =>
            boundary.Name != request.Name ||
            boundary.LookupKey != request.LookupKey;

        static bool HasChanges(ClientApp clientApp, EditBoundaryRequest.ClientAppObj requestClientApp) =>
            clientApp.Name != requestClientApp.Name ||
            clientApp.LookupKey != requestClientApp.LookupKey;
    }
}