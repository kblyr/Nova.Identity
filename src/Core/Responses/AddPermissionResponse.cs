namespace Nova.Identity.Responses
{
    public record AddPermissionResponse
    {
        public int Id { get; init; }
        public string Name { get; init; } = "";
        public string? LookupKey { get; init; }
    }
}