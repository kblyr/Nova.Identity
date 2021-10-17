using CodeCompanion.EntityFrameworkCore;
using CodeCompanion.Processes;
using Microsoft.EntityFrameworkCore;
using Nova.Identity.Data;
using Nova.Identity.Entities;
using Nova.Identity.Exceptions;

namespace Nova.Identity.Processes
{
    sealed class InsertBoundary : IAsyncProcess<Boundary, Boundary>
    {
        public async Task<Boundary> ExecuteAsync(IProcessContext processContext, Boundary boundary, CancellationToken cancellationToken = default)
        {
            if (processContext is IdentityDbContext context)
            {
                if (await NameExistsAsync(context, boundary.Name, cancellationToken))
                    throw new BoundaryNameAlreadyExistsException { Name = boundary.Name };

                if (boundary.LookupKey is not null && await LookupKeyExistsAsync(context, boundary.LookupKey, cancellationToken))
                    throw new BoundaryLookupKeyAlreadyExistsException { LookupKey = boundary.LookupKey };
                    
                context.Boundaries.Add(boundary, context.CurrentFootprint);
                await context.TrySaveChangesAsync(cancellationToken);
                return boundary;
            }

            throw InvalidProcessContextException.Expects<IdentityDbContext>();
        }

        static async Task<bool> NameExistsAsync(IdentityDbContext context, string name, CancellationToken cancellationToken) => await context.Boundaries
            .Where(boundary => boundary.Name == name)
            .AnyAsync(cancellationToken);

        static async Task<bool> LookupKeyExistsAsync(IdentityDbContext context, string lookupKey, CancellationToken cancellationToken) => await context.Boundaries
            .IgnoreQueryFilters()
            .Where(boundary => boundary.LookupKey == lookupKey)
            .AnyAsync(cancellationToken);
    }
}