using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Nova.Identity.Options;
using Nova.Identity.Requests;

namespace Nova.Identity.Validators
{
    public sealed class AddBoundaryValidator : AbstractValidator<AddBoundaryRequest>
    {
        public AddBoundaryValidator(IConfiguration configuration, IOptions<BoundaryOptions> boundaryOptions, IOptions<ClientAppOptions> clientAppOptions, IOptions<RoleOptions> roleOptions, IOptions<PermissionOptions> permissionOptions)
        {
            CascadeMode = configuration.ValidationCascadeMode();
            var boundaryOpts = boundaryOptions.Value;
            var clientAppOpts = clientAppOptions.Value;
            var roleOpts = roleOptions.Value;
            var permissionOpts = permissionOptions.Value;

            RuleFor(request => request.Name).FromOptions(boundaryOpts.Name);
            RuleFor(request => request.LookupKey).FromOptions(boundaryOpts.LookupKey);

            RuleForEach(request => request.ClientApps)
                .ChildRules(requestClientApp => {
                    requestClientApp.CascadeMode = CascadeMode;

                    requestClientApp.RuleFor(requestClientApp => requestClientApp.Name).FromOptions(clientAppOpts.Name);
                    requestClientApp.RuleFor(requestClientApp => requestClientApp.LookupKey).FromOptions(clientAppOpts.LookupKey);
                });

            RuleForEach(request => request.Roles)
                .ChildRules(requestRole => {
                    requestRole.CascadeMode = CascadeMode;

                    requestRole.RuleFor(requestRole => requestRole.Name).FromOptions(roleOpts.Name);
                    requestRole.RuleFor(requestRole => requestRole.LookupKey).FromOptions(roleOpts.LookupKey);
                    requestRole.RuleFor(requestRole => requestRole.PermissionTempIds).Distinct();
                });

            RuleForEach(request => request.Permissions)
                .ChildRules(requestPermission => {
                    requestPermission.CascadeMode = CascadeMode;

                    requestPermission.RuleFor(requestPermission => requestPermission.Name).FromOptions(permissionOpts.Name);
                    requestPermission.RuleFor(requestPermission => requestPermission.LookupKey).FromOptions(permissionOpts.LookupKey);
                });
        }
    }
}