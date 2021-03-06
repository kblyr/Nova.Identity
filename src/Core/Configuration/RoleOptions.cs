using CodeCompanion.Constraints;

namespace Nova.Identity.Configuration
{
    public record RoleOptions
    {
        public const string ConfigKey = "Role";
        public StringPropertyOptions Name { get; init; } = default!;
        public NullableStringPropertyOptions LookupKey { get; init; } = default!;
    }
}