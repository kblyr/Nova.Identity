using FluentValidation;
using Nova.Identity.Requests;

namespace Nova.Identity.Validators
{
    public sealed class AddClientAppValidator : AbstractValidator<AddClientAppRequest>
    {
        public AddClientAppValidator()
        {
            RuleFor(request => request.Name)
                .NotEmpty();
        }
    }
}