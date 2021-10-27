using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Nova.Identity.Options;
using Nova.Identity.Requests;

namespace Nova.Identity.Validators
{
    public sealed class AddRoleValidator : AbstractValidator<AddRoleRequest>
    {
        public AddRoleValidator(IConfiguration configuration, IOptions<RoleOptions> roleOptions)
        {
            CascadeMode = configuration.ValidationCascadeMode();
            var roleOpts = roleOptions.Value;

            RuleFor(request => request.Name).FromOptions(roleOpts.Name);
            RuleFor(request => request.LookupKey).FromOptions(roleOpts.LookupKey);
            RuleForEach(request => request.PermissionIds).NotEqual(0);
        }
    }
}