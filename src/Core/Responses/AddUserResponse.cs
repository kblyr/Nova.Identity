namespace Nova.Identity.Responses
{
    public record AddUserResponse
    {
        public int Id { get; init; }
        public string Username { get; init; } = "";
        public bool IsActive { get; init; }
        public bool IsPasswordChangeRequired { get; init; }
        public IEnumerable<ClientAppObj> ClientApps { get; init; } = Enumerable.Empty<ClientAppObj>();
        public IEnumerable<RoleObj> Roles { get; init; } = Enumerable.Empty<RoleObj>();

        public record BoundaryObj
        {
            public short Id { get; init; }
            public string Name { get; init; } = "";
        }

        public record ClientAppObj
        {
            public short Id { get; init; }
            public string Name { get; init; } = "";
            BoundaryObj? Boundary { get; init; }
        }

        public record RoleObj
        {
            public int Id { get; init; }
            public string Name { get; init; } = "";
            BoundaryObj? Boundary { get; init; }
        }
    }
}