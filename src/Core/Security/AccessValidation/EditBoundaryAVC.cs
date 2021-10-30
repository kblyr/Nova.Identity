using Nova.Common.Security.AccessValidation;
using Nova.Identity.Requests;

namespace Nova.Identity.Security.AccessValidation
{
    sealed class EditBoundaryAVC : IRequestAccessValidationConfiguration<EditBoundaryRequest>
    {
        public void Configure(IRequestAccessValidationContext context, EditBoundaryRequest request)
        {
            context
                .RequirePermission(Permissions.Boundary.Edit)
                .RequirePermissionIf(Permissions.ClientApp.Add, request.ClientApps.Any(clientApp => (clientApp.Id ?? 0) == 0))
                .RequirePermissionIf(Permissions.ClientApp.Edit, request.ClientApps.Any(clientApp => (clientApp.Id ?? 0) != 0))
                .RequirePermissionIf(Permissions.Role.Add, request.Roles.Any(role => (role.Id ?? 0) == 0))
                .RequirePermissionIf(Permissions.Role.Edit, request.Roles.Any(role => (role.Id ?? 0) != 0))
                .RequirePermissionIf(Permissions.Permission.Add, request.Permissions.Any(permmission => (permmission.Id ?? 0) == 0))
                .RequirePermissionIf(Permissions.Permission.Edit, request.Permissions.Any(permission => (permission.Id ?? 0) != 0))
                .RequirePermissionIf(Permissions.RolePermission.Add, request.Roles.Any(role => role.PermissionIds.Any() || role.PermissionTempIds.Any()))
                .RequirePermissionIf(Permissions.ClientApp.Delete, request.DeletedClientAppIds.Any())
                .RequirePermissionIf(Permissions.Role.Delete, request.DeletedRoleIds.Any())
                .RequirePermissionIf(Permissions.Permission.Delete, request.DeletedPermissionIds.Any())
                .RequirePermissionIf(Permissions.RolePermission.Delete, request.Roles.Any(role => role.RemovedPermissionIds.Any()));
        }
    }
}