using FluentValidation;
using Nova.Identity.Options;

namespace Nova.Identity
{
    public static class IRuleBuilderExtensions
    {
        public static IRuleBuilder<T, string> FromOptions<T>(this IRuleBuilder<T, string> builder, StringPropertyOptions options)
        {
            builder.NotNull();

            if (!options.AllowWhiteSpace)
                builder.NotWhiteSpace();

            if (options.MaxLength > 0)
                builder.MaximumLength(options.MaxLength);

            if (!string.IsNullOrEmpty(options.InvalidChars))
                builder.DoesNotContainInvalidChars(options.InvalidChars);

            return builder;
        }

        public static IRuleBuilder<T, string?> FromOptions<T>(this IRuleBuilder<T, string?> builder, NullableStringPropertyOptions options)
        {
            if (!options.AllowWhiteSpace)
                builder.NotWhiteSpace();

            if (options.MaxLength > 0)
                builder.MaximumLength(options.MaxLength);

            if (!string.IsNullOrEmpty(options.InvalidChars))
                builder.DoesNotContainInvalidChars(options.InvalidChars);

            return builder;
        }

        public static IRuleBuilderOptions<T, string?> NotWhiteSpace<T>(this IRuleBuilder<T, string?> builder)
        {
            return builder.Must(value => value is null || !string.IsNullOrWhiteSpace(value))
                .WithErrorCode(nameof(NotWhiteSpace))
                .WithMessage("'{PropertyName}' is empty or white-space");
        }

        public static IRuleBuilderOptions<T, string?> DoesNotContainInvalidChars<T>(this IRuleBuilder<T, string?> builder, string invalidChars)
        {
            var containsInvalidChars = static (string s, string invalidChars) => {
                foreach (var c in s)
                    if (invalidChars.Contains(c))
                        return true;
                return false;
            };

            return builder.Must(value => value is null || !containsInvalidChars(value, invalidChars))
                .WithErrorCode(nameof(DoesNotContainInvalidChars))
                .WithMessage("'{PropertyName}' contains invalid characters");
        }

        public static IRuleBuilderOptions<T, TProperty> NotIn<T, TProperty>(this IRuleBuilder<T, TProperty> builder, IEnumerable<TProperty> invalidValues)
        {
            return builder.Must(value => !invalidValues.Contains(value))
                .WithErrorCode(nameof(NotIn))
                .WithMessage("'{PropertyName}' is invalid");
        }

        public static IRuleBuilderOptions<T, IEnumerable<TProperty>> Distinct<T, TProperty>(this IRuleBuilder<T, IEnumerable<TProperty>> builder)
        {
            return builder.Must(value => value is null || value.Count() == value.Distinct().Count())
                .WithErrorCode(nameof(Distinct))
                .WithMessage("'{PropertyName}' contains duplicate values");
        }

        public static IRuleBuilderOptions<T, IEnumerable<TProperty>> Distinct<T, TProperty>(this IRuleBuilder<T, IEnumerable<TProperty>> builder, IEqualityComparer<TProperty> equalityComparer)
        {
            return builder.Must(value => value is null || value.Count() == value.Distinct(equalityComparer).Count())
                .WithMessage(nameof(Distinct))
                .WithMessage("'{PropertyName}' contains duplicate values");
        }
    }
}