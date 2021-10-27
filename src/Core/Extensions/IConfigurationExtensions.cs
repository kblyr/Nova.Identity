using FluentValidation;
using Microsoft.Extensions.Configuration;

namespace Nova.Identity
{
    public static class IConfigurationExtensions
    {
        public static CascadeMode ValidationCascadeMode(this IConfiguration configuration) 
        {
            if (Enum.TryParse(configuration[nameof(ValidationCascadeMode)], out CascadeMode cascadeMode))
                return cascadeMode;

            return CascadeMode.Continue;
        }
    }
}