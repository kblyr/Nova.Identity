namespace Nova.Identity.Responses
{
    public record EditPermissionResponse
    {
        public int Id { get; init; }
        public string Name { get; init; } = "";
        public string? LookupKey { get; init; }
        public BoundaryObj? Boundary { get; init; }
        
        public record BoundaryObj
        {
            public short Id { get; init; }
            public string Name { get; init; } = "";
        }
    }
}