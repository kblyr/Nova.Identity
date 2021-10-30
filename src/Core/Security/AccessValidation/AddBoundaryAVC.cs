using Nova.Common.Security.AccessValidation;
using Nova.Identity.Requests;

namespace Nova.Identity.Security.AccessValidation
{
    sealed class AddBoundaryAVC : IRequestAccessValidationConfiguration<AddBoundaryRequest>
    {
        public void Configure(IRequestAccessValidationContext context, AddBoundaryRequest request)
        {
            context
                .RequirePermission(Permissions.Boundary.Add)
                .RequirePermissionIf(Permissions.ClientApp.Add, request.ClientApps.Any())
                .RequirePermissionIf(Permissions.Role.Add, request.Roles.Any())
                .RequirePermissionIf(Permissions.Permission.Add, request.Permissions.Any())
                .RequirePermissionIf(Permissions.RolePermission.Add, request.Roles.Any(role => role.PermissionTempIds.Any()));
        }
    }
}