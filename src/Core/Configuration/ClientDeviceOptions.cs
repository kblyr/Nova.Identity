using CodeCompanion.Constraints;

namespace Nova.Identity.Configuration
{
    public record ClientDeviceOptions
    {
        public StringPropertyOptions IpAddress { get; init; } = default!;
        public StringPropertyOptions Name { get; init; } = default!;
    }
}