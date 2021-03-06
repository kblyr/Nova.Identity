using CodeCompanion.Extensions.FluentValidation;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Nova.Identity.Configuration;
using Nova.Identity.Requests;

namespace Nova.Identity.Validators
{
    public sealed class AddPermissionValidator : AbstractValidator<AddPermissionRequest>
    {
        public AddPermissionValidator(IConfiguration configuration, IOptions<PermissionOptions> permissionOptions)
        {
            CascadeMode = configuration.ValidationCascadeMode();
            var permissionOpts = permissionOptions.Value;

            RuleFor(request => request.Name).FromOptions(permissionOpts.Name);
            RuleFor(request => request.LookupKey).FromOptions(permissionOpts.LookupKey);
            RuleFor(request => request.BoundaryId).NotEqual((short)0);
        }
    }
}