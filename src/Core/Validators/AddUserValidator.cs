using CodeCompanion.Extensions.FluentValidation;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Nova.Identity.Configuration;
using Nova.Identity.Requests;

namespace Nova.Identity.Validators
{
    public sealed class AddUserValidator : AbstractValidator<AddUserRequest>
    {
        public AddUserValidator(IConfiguration configuration, IOptions<UserOptions> userOptions)
        {
            CascadeMode = configuration.ValidationCascadeMode();
            var userOpts = userOptions.Value;

            RuleFor(request => request.Username).FromOptions(userOpts.Username);
            RuleFor(request => request.Password).FromOptions(userOpts.Password);
            RuleForEach(request => request.ClientAppIds).NotEqual((short)0);
            RuleForEach(request => request.RoleIds).NotEqual(0);
        }
    }
}