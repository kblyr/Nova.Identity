using CodeCompanion.EntityFrameworkCore;
using CodeCompanion.Processes;
using Microsoft.EntityFrameworkCore;
using Nova.Identity.Data;
using Nova.Identity.Entities;
using Nova.Identity.Exceptions;

namespace Nova.Identity.Processes
{
    sealed class UpdateBoundary : IAsyncProcess<Boundary, Boundary>
    {
        public async Task<Boundary> ExecuteAsync(IProcessContext processContext, Boundary boundary, CancellationToken cancellationToken = default)
        {
            if (processContext is IdentityDbContext context)
            {
                if (await NameExistsAsync(context, boundary.Id, boundary.Name, cancellationToken))
                    throw new BoundaryNameAlreadyExistsException { Name = boundary.Name };

                if (boundary.LookupKey is not null && await LookupKeyExistsAsync(context, boundary.Id, boundary.Name, cancellationToken))
                    throw new BoundaryLookupKeyAlreadyExistsException { LookupKey = boundary.LookupKey };

                context.Boundaries.Update(boundary, context.CurrentFootprint);
                return boundary;
            }

            throw InvalidProcessContextException.Expects<IdentityDbContext>();
        }

        static async Task<bool> NameExistsAsync(IdentityDbContext context, short id, string name, CancellationToken cancellationToken) => await context.Boundaries
            .Where(boundary => boundary.Id != id)
            .Where(boundary => boundary.Name == name)
            .AnyAsync(cancellationToken);

        static async Task<bool> LookupKeyExistsAsync(IdentityDbContext context, short id, string lookupKey, CancellationToken cancellationToken) => await context.Boundaries
            .IgnoreQueryFilters()
            .Where(boundary => boundary.Id != id)
            .Where(boundary => boundary.LookupKey == lookupKey)
            .AnyAsync(cancellationToken);
    }
}