using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Nova.Identity.Options;
using Nova.Identity.Requests;

namespace Nova.Identity.Validators
{
    public sealed class AddClientAppValidator : AbstractValidator<AddClientAppRequest>
    {
        public AddClientAppValidator(IConfiguration configuration, IOptions<ClientAppOptions> clientAppOptions)
        {
            CascadeMode = configuration.ValidationCascadeMode();
            var clientApp = clientAppOptions.Value;

            RuleFor(request => request.Name).FromOptions(clientApp.Name);
            RuleFor(request => request.LookupKey).FromOptions(clientApp.LookupKey);
        }
    }
}