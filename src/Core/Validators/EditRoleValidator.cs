using CodeCompanion.Extensions.FluentValidation;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Nova.Identity.Configuration;
using Nova.Identity.Requests;

namespace Nova.Identity.Validators
{
    public sealed class EditRoleValidator : AbstractValidator<EditRoleRequest>
    {
        public EditRoleValidator(IConfiguration configuration, IOptions<RoleOptions> roleOptions)
        {
            CascadeMode = configuration.ValidationCascadeMode();
            var roleOpts = roleOptions.Value;

            RuleFor(request => request.Id).NotEqual(0);
            RuleFor(request => request.Name).FromOptions(roleOpts.Name);
            RuleFor(request => request.LookupKey).FromOptions(roleOpts.LookupKey);
            RuleFor(request => request.BoundaryId).NotEqual((short)0);
            RuleForEach(request => request.PermissionIds).NotEqual(0);
            RuleFor(request => request.PermissionIds).Distinct();
            RuleForEach(request => request.RemovedPermissionIds).NotEqual(0);
            RuleFor(request => request.RemovedPermissionIds).Distinct();
        }
    }
}