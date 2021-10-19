using FluentValidation;
using Nova.Identity.Requests;

namespace Nova.Identity.Validators
{
    public sealed class AddBoundaryValidator : AbstractValidator<AddBoundaryRequest>
    {
        public AddBoundaryValidator()
        {
            RuleFor(request => request.Name)
                .NotEmpty();
        }
    }
}