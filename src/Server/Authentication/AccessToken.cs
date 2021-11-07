namespace Nova.Identity.Authentication
{
    public record AccessToken
    {
        public string TokenString { get; init; } = "";
    }
}