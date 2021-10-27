using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Nova.Identity.Options;
using Nova.Identity.Requests;

namespace Nova.Identity.Validators
{
    public sealed class EditBoundaryValidator : AbstractValidator<EditBoundaryRequest>
    {
        public EditBoundaryValidator(IConfiguration configuration, IOptions<BoundaryOptions> boundaryOptions, IOptions<ClientAppOptions> clientAppOptions, IOptions<RoleOptions> roleOptions, IOptions<PermissionOptions> permissionOptions)
        {
            CascadeMode = configuration.ValidationCascadeMode();
            var boundaryOpts = boundaryOptions.Value;
            var clientAppOpts = clientAppOptions.Value;
            var roleOpts = roleOptions.Value;
            var permissionOpts = permissionOptions.Value;

            RuleFor(request => request.Id).NotEqual((short)0);
            RuleFor(request => request.Name).FromOptions(boundaryOpts.Name);
            RuleFor(request => request.LookupKey).FromOptions(boundaryOpts.LookupKey);

            RuleForEach(request => request.ClientApps)
                .ChildRules(requestClientApp => {
                    requestClientApp.CascadeMode = CascadeMode;

                    requestClientApp.RuleFor(requestClientApp => requestClientApp.Id).NotEqual((short)0);
                    requestClientApp.RuleFor(requestClientApp => requestClientApp.Name).FromOptions(clientAppOpts.Name);
                    requestClientApp.RuleFor(requestClientApp => requestClientApp.LookupKey).FromOptions(clientAppOpts.LookupKey);
                });

            RuleFor(request => request.ClientApps).DistinctBy(requestClientApp => requestClientApp.Id);

            RuleForEach(request => request.Roles)
                .ChildRules(requestRole => {
                    requestRole.CascadeMode = CascadeMode;

                    requestRole.RuleFor(requestRole => requestRole.Id).NotEqual(0);
                    requestRole.RuleFor(requestRole => requestRole.Name).FromOptions(roleOpts.Name);
                    requestRole.RuleFor(requestRole => requestRole.LookupKey).FromOptions(roleOpts.LookupKey);
                    requestRole.RuleForEach(requestRole => requestRole.PermissionIds).NotEqual(0);
                    requestRole.RuleFor(requestRole => requestRole.PermissionIds).Distinct();
                    requestRole.RuleForEach(requestRole => requestRole.PermissionTempIds).NotEqual(0);
                    requestRole.RuleFor(requestRole => requestRole.PermissionTempIds).Distinct();
                    requestRole.RuleForEach(requestRole => requestRole.RemovedPermissionIds).NotEqual(0);
                    requestRole.RuleFor(requestRole => requestRole.RemovedPermissionIds).Distinct();
                });

            RuleForEach(request => request.Permissions)
                .ChildRules(requestPermission => {
                    requestPermission.CascadeMode = CascadeMode;

                    requestPermission.RuleFor(requestPermission => requestPermission.Id).NotEqual(0);
                    requestPermission.RuleFor(requestPermission => requestPermission.Name).FromOptions(permissionOpts.Name);
                    requestPermission.RuleFor(requestPermission => requestPermission.LookupKey).FromOptions(permissionOpts.LookupKey);
                });

            RuleForEach(request => request.DeletedClientAppIds).NotEqual((short)0);
            RuleFor(request => request.DeletedClientAppIds).Distinct();
            RuleForEach(request => request.DeletedRoleIds).NotEqual(0);
            RuleFor(request => request.DeletedRoleIds).Distinct();
            RuleForEach(request => request.DeletedPermissionIds).NotEqual(0);
            RuleFor(request => request.DeletedPermissionIds).Distinct();
        }
    }
}