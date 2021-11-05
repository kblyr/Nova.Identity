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
        // CodeCompanion.Processes.DependencyInjection
        private static IEnumerable<Type> GetSpecificImplemenetationTypes(Assembly containingAssembly, params Type[] genericInterfaceTypes) => containingAssembly.GetTypes()
            .Where(type =>
                !type.IsGenericTypeDefinition &&
                !type.ContainsGenericParameters &&
                type.GetInterfaces().Any(interfaceType => genericInterfaceTypes.Contains(interfaceType.IsConstructedGenericType ? interfaceType.GetGenericTypeDefinition() : interfaceType))
            );

        private static IServiceCollection AddImplementation(this IServiceCollection services, Type implementation, ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            services.Add(new ServiceDescriptor(implementation, implementation, lifetime));
            return services;
        }

        public static IServiceCollection AddProcesses(this IServiceCollection services, Assembly containingAssembly, ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            foreach (var processType in GetSpecificImplemenetationTypes(containingAssembly, typeof(IProcess<,>), typeof(IAsyncProcess<,>)))
            {
                services.AddImplementation(processType, lifetime);
            }

            return services;
        }

        // Nova.Common.Security.AccessValidation
        static readonly Type _genericType_IRequestAccessValidationConfiguration = typeof(IRequestAccessValidationConfiguration<>);
        public static IServiceCollection AddRequestAccessValidationConfigurations(this IServiceCollection services, Assembly assemblyMarker, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            foreach (var implementation in assemblyMarker.GetSpecificImplementationTypesOfGenericInterfaceType(_genericType_IRequestAccessValidationConfiguration))
            {
                var specificInterfaceType = implementation.GetSpecificInterfaceType(_genericType_IRequestAccessValidationConfiguration);
                services.Add(new ServiceDescriptor(specificInterfaceType, implementation, lifetime));
            }

            return services;
        }

        public static IEnumerable<Type> GetSpecificImplementationTypesOfGenericInterfaceType(this Assembly assembly, Type genericInterfaceType) => assembly.GetTypes()
            .Where(type =>
                !type.IsGenericTypeDefinition &&
                !type.ContainsGenericParameters &&
                type.IsClass &&
                type.GetInterfaces().Any(interfaceType => (interfaceType.IsConstructedGenericType? interfaceType.GetGenericTypeDefinition() : interfaceType).Equals(genericInterfaceType))
            );

        public static Type GetSpecificInterfaceType(this Type type, Type genericInterfaceType) => type.GetInterfaces()
            .Single(interfaceType => (interfaceType.IsConstructedGenericType? interfaceType.GetGenericTypeDefinition() : interfaceType).Equals(genericInterfaceType));
    }
}