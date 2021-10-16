namespace Nova.Identity.Responses
{
    public record AddClientAppResponse
    {
        public short Id { get; init; }
        public string Name { get; init; } = "";
        public string? LookupKey { get; init; } = "";
    }
}