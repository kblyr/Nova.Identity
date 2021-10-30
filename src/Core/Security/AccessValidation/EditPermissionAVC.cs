using Nova.Common.Security.AccessValidation;
using Nova.Identity.Requests;

namespace Nova.Identity.Security.AccessValidation
{
    sealed class EditPermissionAVC : IRequestAccessValidationConfiguration<EditPermissionRequest>
    {
        public void Configure(IRequestAccessValidationContext context, EditPermissionRequest request)
        {
            context.RequirePermission(Permissions.Permission.Edit);
        }
    }
}