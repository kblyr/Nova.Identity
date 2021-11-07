using CodeCompanion.Constraints;

namespace Nova.Identity.Configuration
{
    public record PermissionOptions
    {
        public const string ConfigKey = "Permission";
        public StringPropertyOptions Name { get; init; } = default!;
        public NullableStringPropertyOptions LookupKey { get; init; } = default!;
    }
}