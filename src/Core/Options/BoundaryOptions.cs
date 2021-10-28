using CodeCompanion.Constraints;

namespace Nova.Identity.Options
{
    public record BoundaryOptions
    {
        public const string ConfigKey = "Boundary";
        
        public StringPropertyOptions Name { get; init; } = default!;
        public NullableStringPropertyOptions LookupKey { get; init; } = default!;
    }
}