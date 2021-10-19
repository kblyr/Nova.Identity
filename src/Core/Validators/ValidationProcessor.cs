using FluentValidation;
using FluentValidation.Results;
using MediatR.Pipeline;

namespace Nova.Identity.Validators
{
    sealed class ValidationProcessor<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
    {
        readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationProcessor(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request);
            var failures = new List<ValidationFailure>();

            foreach (var validator in _validators)
            {
                var result = await validator.ValidateAsync(context, cancellationToken);

                if (result.Errors is not null)
                    failures.AddRange(result.Errors);
            }

            if (failures.Any())
                throw new ValidationException(failures);
        }
    }
}