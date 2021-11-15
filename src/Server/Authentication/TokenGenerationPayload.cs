namespace Nova.Identity.Authentication
{
    public record TokenGenerationPayload
    {
        public string ClientDeviceId { get; init; } = "";
        public string UserId { get; init; } = "";
        public string Username { get; init; } = "";
        public string? BoundaryId { get; init; }
        public string? ClientAppId { get; init; }
        public IEnumerable<string> Roles { get; init; } = Enumerable.Empty<string>();
        public IEnumerable<string> Permissions { get; init; } = Enumerable.Empty<string>();
    }
}