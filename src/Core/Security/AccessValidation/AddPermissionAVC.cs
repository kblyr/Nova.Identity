using Nova.Common.Security.AccessValidation;
using Nova.Identity.Requests;

namespace Nova.Identity.Security.AccessValidation
{
    sealed class AddPermissionAVC : IRequestAccessValidationConfiguration<AddPermissionRequest>
    {
        public void Configure(IRequestAccessValidationContext context, AddPermissionRequest request)
        {
            context.RequirePermission(Permissions.Permission.Add);
        }
    }
}