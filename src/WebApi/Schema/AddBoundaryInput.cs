using Nova.Identity.Requests;

namespace Nova.Identity.Schema
{
    [SchemaId(SchemaIds.Boundary.Add.Input)]
    public record AddBoundaryInput
    {
        public string Name { get; init; } = "";
        public string? LookupKey { get; init; }
        public IEnumerable<ClientAppObj> ClientApps { get; init; } = Enumerable.Empty<ClientAppObj>();
        public IEnumerable<RoleObj> Roles { get; init; } = Enumerable.Empty<RoleObj>();
        public IEnumerable<PermissionObj> Permissions { get; init; } = Enumerable.Empty<PermissionObj>();

        public AddBoundaryRequest ToRequest() => new()
        {
            Name = Name,
            LookupKey = LookupKey,
            ClientApps = ClientApps.Select(clientApp => new AddBoundaryRequest.ClientAppObj
            {
                Name = clientApp.Name,
                LookupKey = clientApp.LookupKey
            }),
            Roles = Roles.Select(role => new AddBoundaryRequest.RoleObj
            {
                Name = role.Name,
                LookupKey = role.LookupKey,
                PermissionTempIds = role.PermissionTempIds
            }),
            Permissions = Permissions.Select(permission => new AddBoundaryRequest.PermissionObj
            {
                TempId = permission.TempId,
                Name = permission.Name,
                LookupKey = permission.LookupKey
            })
        };

        [SchemaId(SchemaIds.Boundary.Add.Input_ClientApp)]
        public record ClientAppObj
        {
            public string Name { get; init; } = "";
            public string? LookupKey { get; init; }
        }

        [SchemaId(SchemaIds.Boundary.Add.Input_Role)]
        public record RoleObj
        {
            public string Name { get; init; } = "";
            public string? LookupKey { get; init; }
            public IEnumerable<int> PermissionTempIds { get; init; } = Enumerable.Empty<int>();
        }

        [SchemaId(SchemaIds.Boundary.Add.Input_Permission)]
        public record PermissionObj
        {
            public int TempId { get; init; }
            public string Name { get; init; } = "";
            public string? LookupKey { get; init; }
        }
    }
}