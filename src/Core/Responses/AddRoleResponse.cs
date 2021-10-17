namespace Nova.Identity.Responses
{
    public record AddRoleResponse
    {
        public int Id { get; init; }
        public string Name { get; init; } = "";
        public string? LookupKey { get; init; }
        public BoundaryObj? Boundary { get; init; }
        public IEnumerable<PermissionObj> Permissions { get; init; } = Enumerable.Empty<PermissionObj>();

        public record BoundaryObj
        {
            public short Id { get; init; }
            public string Name { get; init; } = "";
        }

        public record PermissionObj
        {
            public int Id { get; init; }
            public string Name { get; init; } = "";
        }
    }
}