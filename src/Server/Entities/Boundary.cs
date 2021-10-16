namespace Nova.Identity.Entities
{
    record Boundary
    {
        public short Id { get; init; }
        public string Name { get; init; } = "";
        public string? LookupKey { get; init; }
    }
}