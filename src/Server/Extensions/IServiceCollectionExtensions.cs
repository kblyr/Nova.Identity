using CodeCompanion.Processes;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Nova.Identity.DependencyInjection;

namespace Nova.Identity
{
    public static class IServiceCollectionExtensions
    {
        public static NovaIdentityDependencyBuilder AddNovaIdentity(this IServiceCollection services)
        {
            services
                .AddMediatR(CoreAssemblyMarker.Assembly, ServerAssemblyMarker.Assembly)
                .AddProcessImplementations(ServerAssemblyMarker.Assembly);
            return new(services);
        }
    }
}