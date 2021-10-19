using CodeCompanion.Processes;
using FluentValidation;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using Nova.Identity.DependencyInjection;
using Nova.Identity.Validators;

namespace Nova.Identity
{
    public static class IServiceCollectionExtensions
    {
        public static NovaIdentityDependencyBuilder AddNovaIdentity(this IServiceCollection services)
        {
            services
                .AddMediatR(CoreAssemblyMarker.Assembly, ServerAssemblyMarker.Assembly)
                .AddProcessImplementations(ServerAssemblyMarker.Assembly)
                .AddValidatorsFromAssembly(ServerAssemblyMarker.Assembly)
                .AddScoped(typeof(IRequestPreProcessor<>), typeof(ValidationProcessor<>));
            return new(services);
        }
    }
}