using Nova.Common.Security.AccessValidation;
using Nova.Identity.Requests;

namespace Nova.Identity.Security.AccessValidation
{
    sealed class EditClientAppAVC : IRequestAccessValidationConfiguration<EditClientAppRequest>
    {
        public void Configure(IRequestAccessValidationContext context, EditClientAppRequest request)
        {
            context.RequirePermission(Permissions.ClientApp.Edit);
        }
    }
}