using CodeCompanion.EntityFrameworkCore;
using CodeCompanion.Processes;
using Microsoft.EntityFrameworkCore;
using Nova.Identity.Data;
using Nova.Identity.Entities;
using Nova.Identity.Exceptions;

namespace Nova.Identity.Processes
{
    sealed class InsertClientApp : IAsyncProcess<ClientApp, ClientApp>
    {
        public async Task<ClientApp> ExecuteAsync(IProcessContext processContext, ClientApp clientApp, CancellationToken cancellationToken = default)
        {
            if (processContext is IdentityDbContext context)
            {
                if (await NameExistsAsync(context, clientApp.Name, clientApp.BoundaryId, cancellationToken))
                    throw new ClientAppNameAlreadyExistsException { Name = clientApp.Name };

                if (clientApp.LookupKey is not null && await LookupKeyExistsAsync(context, clientApp.LookupKey, cancellationToken))
                    throw new ClientAppLookupKeyAlreadyExistsException { LookupKey = clientApp.LookupKey };
                
                context.ClientApps.Add(clientApp, context.CurrentFootprint);
                await context.TrySaveChangesAsync(cancellationToken);
                return clientApp;
            }

            throw InvalidProcessContextException.Expects<IdentityDbContext>();
        }

        static async Task<bool> NameExistsAsync(IdentityDbContext context, string name, int? boundaryId, CancellationToken cancellationToken) => await context.ClientApps
            .Where(clientApp => clientApp.Name == name)
            .Where(clientApp => clientApp.BoundaryId == boundaryId)
            .AnyAsync(cancellationToken);

        static async Task<bool> LookupKeyExistsAsync(IdentityDbContext context, string lookupKey, CancellationToken cancellationToken) => await context.ClientApps
            .IgnoreQueryFilters()
            .Where(clientApp => clientApp.LookupKey == lookupKey)
            .AnyAsync(cancellationToken);
    }
}