using Nova.Identity.Responses;

namespace Nova.Identity.Schema
{
    [SchemaId(SchemaIds.Boundary.Add.Output)]
    public record AddBoundaryOutput
    {
        public short Id { get; init; }
        public string Name { get; init; } = "";
        public string? LookupKey { get; init; }
        public IEnumerable<ClientAppObj> ClientApps { get; init; } = Enumerable.Empty<ClientAppObj>();
        public IEnumerable<RoleObj> Roles { get; init; } = Enumerable.Empty<RoleObj>();
        public IEnumerable<PermissionObj> Permissions { get; init; } = Enumerable.Empty<PermissionObj>();

        [SchemaId(SchemaIds.Boundary.Add.Output_ClientApp)]
        public record ClientAppObj
        {
            public short Id { get; init; }
            public string Name { get; init; } = "";
            public string? LookupKey { get; init; }
        }

        [SchemaId(SchemaIds.Boundary.Add.Output_Role)]
        public record RoleObj
        {
            public int Id { get; init; }
            public string Name { get; init; } = "";
            public string? LookupKey { get; init; }
        }

        [SchemaId(SchemaIds.Boundary.Add.Output_Permisson)]
        public record PermissionObj
        {
            public int Id { get; init; }
            public string Name { get; init; } = "";
            public string? LookupKey { get; init; }
        }

        public static AddBoundaryOutput From(AddBoundaryResponse response) => new()
        {
            Id = response.Id,
            Name = response.Name,
            LookupKey = response.LookupKey,
            ClientApps = response.ClientApps.Select(clientApp => new ClientAppObj
            {
                Id = clientApp.Id,
                Name = clientApp.Name,
                LookupKey = clientApp.LookupKey
            }),
            Roles = response.Roles.Select(role => new RoleObj
            {
                Id = role.Id,
                Name = role.Name,
                LookupKey = role.LookupKey
            }),
            Permissions = response.Permissions.Select(permission => new PermissionObj
            {
                Id = permission.Id,
                Name = permission.Name,
                LookupKey = permission.LookupKey
            })
        };
    }
}