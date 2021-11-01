using Microsoft.Extensions.DependencyInjection;

namespace Nova.Identity.DependencyInjection
{
    public class NovaIdentityDependencyBuilder
    {
        public IServiceCollection Services { get; }

        public NovaIdentityDependencyBuilder(IServiceCollection services)
        {
            Services = services;
        }
    }
}