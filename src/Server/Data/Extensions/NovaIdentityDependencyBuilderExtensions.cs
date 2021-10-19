using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nova.Identity.DependencyInjection;

namespace Nova.Identity.Data
{
    public static class NovaIdentityDependencyBuilderExtensions
    {
        public static NovaIdentityDependencyBuilder AddData(this NovaIdentityDependencyBuilder builder, string connectionString)
        {
            builder.Services.AddDbContextFactory<IdentityDbContext>(options => options.UseNpgsql(connectionString), ServiceLifetime.Scoped);
            return builder;
        }
    }
}