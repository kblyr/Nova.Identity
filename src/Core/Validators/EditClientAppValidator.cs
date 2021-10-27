using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Nova.Identity.Options;
using Nova.Identity.Requests;

namespace Nova.Identity.Validators
{
    public sealed class EditClientAppValidator : AbstractValidator<EditClientAppRequest>
    {
        public EditClientAppValidator(IConfiguration configuration, IOptions<ClientAppOptions> clientAppOptions)
        {
            CascadeMode = configuration.ValidationCascadeMode();
            var clientAppOpts = clientAppOptions.Value;

            RuleFor(request => request.Id).NotEqual((short)0);
            RuleFor(request => request.Name).FromOptions(clientAppOpts.Name);
            RuleFor(request => request.LookupKey).FromOptions(clientAppOpts.LookupKey);
            RuleFor(request => request.BoundaryId).NotEqual((short)0);
        }
    }
}