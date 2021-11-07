using CodeCompanion.Constraints;

namespace Nova.Identity.Configuration
{
    public record UserOptions
    {
        public const string ConfigKey = "User";
        public StringPropertyOptions Username { get; init; } = default!;
        public StringPropertyOptions Password { get; init; } = default!;
    }
}