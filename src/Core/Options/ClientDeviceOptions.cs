using CodeCompanion.Constraints;

namespace Nova.Identity.Options
{
    public record ClientDeviceOptions
    {
        public StringPropertyOptions IpAddress { get; init; } = default!;
        public StringPropertyOptions Name { get; init; } = default!;
    }
}