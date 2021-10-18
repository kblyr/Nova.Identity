namespace Nova.Identity.Responses
{
    public record EditBoundaryResponse
    {
        public short Id { get; init; }
        public string Name { get; init; } = "";
        public string? LookupKey { get; init; }
        public IEnumerable<ClientAppObj> ClientApps { get; init; } = Enumerable.Empty<ClientAppObj>();
        public IEnumerable<RoleObj> Roles { get; init; } = Enumerable.Empty<RoleObj>();
        public IEnumerable<PermissionObj> Permissions { get; init; } = Enumerable.Empty<PermissionObj>();

        public record ClientAppObj
        {
            public short Id { get; init; }
            public string Name { get; init; } = "";
            public string? LookupKey { get; init; }
        }

        public record RoleObj
        {
            public int Id { get; init; }
            public string Name { get; init; } = "";
            public string? LookupKey { get; init; }
        }

        public record PermissionObj
        {
            public int Id { get; init; }
            public string Name { get; init; } = "";
            public string? LookupKey { get; init; }
        }
    }
}