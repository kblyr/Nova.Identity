using CodeCompanion.Constraints;

namespace Nova.Identity.Configuration
{
    public record ClientAppOptions
    {
        public const string ConfigKey = "ClientApp";

        public StringPropertyOptions Name { get; init; } = default!;
        public NullableStringPropertyOptions LookupKey { get; init; } = default!;
    }
}