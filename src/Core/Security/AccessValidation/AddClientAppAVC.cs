using Nova.Common.Security.AccessValidation;
using Nova.Identity.Requests;

namespace Nova.Identity.Security.AccessValidation
{
    sealed class AddClientAppAVC : IRequestAccessValidationConfiguration<AddClientAppRequest>
    {
        public void Configure(IRequestAccessValidationContext context, AddClientAppRequest request)
        {
            context.RequirePermission(Permissions.ClientApp.Add);
        }
    }
}