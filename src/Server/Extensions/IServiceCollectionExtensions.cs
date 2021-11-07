using System.Reflection;
using CodeCompanion.Processes;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Nova.Common.Security.AccessValidation;
using Nova.Identity.DependencyInjection;

namespace Nova.Identity
{
    public static class IServiceCollectionExtensions
    {
        public static NovaIdentityDependencyBuilder AddNovaIdentity(this IServiceCollection services)
        {
            services
                .AddMediatR(CoreAssemblyMarker.Assembly, ServerAssemblyMarker.Assembly)
                .AddProcesses(ServerAssemblyMarker.Assembly)
                .AddValidatorsFromAssembly(CoreAssemblyMarker.Assembly)
                .AddRequestAccessValidationConfigurations(CoreAssemblyMarker.Assembly);
            return new(services);
        }
    }
}