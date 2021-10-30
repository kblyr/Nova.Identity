using Nova.Common.Schema;
using Nova.Identity.Responses;

namespace Nova.Identity.Schema
{
    [SchemaId(SchemaIds.Boundary.Edit.Output)]
    public record EditBoundaryOutput
    {
        public short Id { get; init; }
        public string Name { get; init; } = "";
        public string? LookupKey { get; init; }
        public IEnumerable<ClientAppObj> ClientApps { get; init; } = Enumerable.Empty<ClientAppObj>();
        public IEnumerable<RoleObj> Roles { get; init; } = Enumerable.Empty<RoleObj>();
        public IEnumerable<PermissionObj> Permissions { get; init; } = Enumerable.Empty<PermissionObj>();

        public static EditBoundaryOutput From(EditBoundaryResponse response) => new()
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

        [SchemaId(SchemaIds.Boundary.Edit.Output_ClientApp)]
        public record ClientAppObj
        {
            public short Id { get; init; }
            public string Name { get; init; } = "";
            public string? LookupKey { get; init; }
        }

        [SchemaId(SchemaIds.Boundary.Edit.Output_Role)]
        public record RoleObj
        {
            public int Id { get; init; }
            public string Name { get; init; } = "";
            public string? LookupKey { get; init; }
        }

        [SchemaId(SchemaIds.Boundary.Edit.Output_Permission)]
        public record PermissionObj
        {
            public int Id { get; init; }
            public string Name { get; init; } = "";
            public string? LookupKey { get; init; }
        }
    }
}