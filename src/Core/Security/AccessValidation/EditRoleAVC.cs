using Nova.Common.Security.AccessValidation;
using Nova.Identity.Requests;

namespace Nova.Identity.Security.AccessValidation
{
    sealed class EditRoleAVC : IRequestAccessValidationConfiguration<EditRoleRequest>
    {
        public void Configure(IRequestAccessValidationContext context, EditRoleRequest request)
        {
            context
                .RequirePermission(Permissions.Role.Edit)
                .RequirePermissionIf(Permissions.RolePermission.Add, request.PermissionIds.Any())
                .RequirePermissionIf(Permissions.RolePermission.Delete, request.RemovedPermissionIds.Any());
        }
    }
}