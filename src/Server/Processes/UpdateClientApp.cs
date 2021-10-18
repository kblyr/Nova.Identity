using CodeCompanion.EntityFrameworkCore;
using CodeCompanion.Processes;
using Microsoft.EntityFrameworkCore;
using Nova.Identity.Data;
using Nova.Identity.Entities;
using Nova.Identity.Exceptions;

namespace Nova.Identity.Processes
{
    sealed class UpdateClientApp : IAsyncProcess<ClientApp, ClientApp>
    {
        public async Task<ClientApp> ExecuteAsync(IProcessContext processContext, ClientApp clientApp, CancellationToken cancellationToken = default)
        {
            if (processContext is IdentityDbContext context)
            {
                if (await NameExistsAsync(context, clientApp.Id, clientApp.Name, cancellationToken))
                    throw new ClientAppNameAlreadyExistsException { Name = clientApp.Name };

                if(clientApp.LookupKey is not null && await LookupKeyExistsAsync(context, clientApp.Id, clientApp.LookupKey, cancellationToken))
                    throw new ClientAppLookupKeyAlreadyExistsException{ LookupKey = clientApp.LookupKey };
                
                context.ClientApps.Update(clientApp, context.CurrentFootprint);
                return clientApp;
            }

            throw InvalidProcessContextException.Expects<IdentityDbContext>();
        }

        static async Task<bool> NameExistsAsync(IdentityDbContext context, short id, string name, CancellationToken cancellationToken) => await context.ClientApps
            .Where(clientApp => clientApp.Id != id)
            .Where(clientApp => clientApp.Name == name)
            .AnyAsync(cancellationToken);

        static async Task<bool> LookupKeyExistsAsync(IdentityDbContext context, short id, string lookupKey, CancellationToken cancellationToken) => await context.ClientApps
            .IgnoreQueryFilters()
            .Where(clientApp => clientApp.Id != id)
            .Where(clientApp => clientApp.LookupKey == lookupKey)
            .AnyAsync(cancellationToken);
    }
}