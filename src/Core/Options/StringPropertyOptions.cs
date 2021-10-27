namespace Nova.Identity.Options
{
    public record StringPropertyOptions
    {
        public int MaxLength { get; init; }
        public string? InvalidChars { get; init; }
        public bool AllowWhiteSpace { get; init; }
    }
}