using Nova.Common.Security.AccessValidation;
using Nova.Identity.Requests;

namespace Nova.Identity.Security.AccessValidation
{
    sealed class AddRoleAVC : IRequestAccessValidationConfiguration<AddRoleRequest>
    {
        public void Configure(IRequestAccessValidationContext context, AddRoleRequest request)
        {
            context
                .RequirePermission(Permissions.Role.Add)
                .RequirePermissionIf(Permissions.RolePermission.Add, request.PermissionIds.Any());
        }
    }
}