using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;

namespace Nova.Identity.Validators
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddValidationProcessor(this IServiceCollection services) => services
                .AddScoped(typeof(IRequestPreProcessor<>), typeof(ValidationProcessor<>));
    }
}