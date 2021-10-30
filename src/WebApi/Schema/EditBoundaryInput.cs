using Nova.Common.Schema;
using Nova.Identity.Requests;

namespace Nova.Identity.Schema
{
    [SchemaId(SchemaIds.Boundary.Edit.Input)]
    public record EditBoundaryInput
    {
        public string Name { get; init; } = "";
        public string? LookupKey { get; init; }
        public IEnumerable<ClientAppObj> ClientApps { get; init; } = Enumerable.Empty<ClientAppObj>();
        public IEnumerable<RoleObj> Roles { get; init; } = Enumerable.Empty<RoleObj>();
        public IEnumerable<PermissionObj> Permissions { get; init; } = Enumerable.Empty<PermissionObj>();
        public IEnumerable<short> DeletedClientAppIds { get; init; } = Enumerable.Empty<short>();
        public IEnumerable<int> DeletedRoleIds { get; init; } = Enumerable.Empty<int>();
        public IEnumerable<int> DeletedPermissionIds { get; init; } = Enumerable.Empty<int>();

        public EditBoundaryRequest ToRequest(short id) => new()
        {
            Id = id,
            Name = Name,
            LookupKey = LookupKey,
            ClientApps = ClientApps.Select(clientApp => new EditBoundaryRequest.ClientAppObj
            {
                Id = clientApp.Id,
                Name = clientApp.Name,
                LookupKey = clientApp.LookupKey
            }),
            Roles = Roles.Select(role => new EditBoundaryRequest.RoleObj
            {
                Id = role.Id,
                Name = role.Name,
                LookupKey = role.LookupKey,
                PermissionIds = role.PermissionIds,
                PermissionTempIds = role.PermissionTempIds,
                RemovedPermissionIds = role.RemovedPermissionIds
            }),
            Permissions = Permissions.Select(permission => new EditBoundaryRequest.PermissionObj
            {
                Id = permission.Id,
                Name = permission.Name,
                LookupKey = permission.LookupKey
            }),
            DeletedClientAppIds = DeletedClientAppIds,
            DeletedRoleIds = DeletedRoleIds,
            DeletedPermissionIds = DeletedPermissionIds
        };

        [SchemaId(SchemaIds.Boundary.Edit.Input_ClientApp)]
        public record ClientAppObj
        {
            public short? Id { get; init; }
            public string Name { get; init; } = "";
            public string? LookupKey { get; init; }
        }

        [SchemaId(SchemaIds.Boundary.Edit.Input_Role)]
        public record RoleObj
        {
            public int? Id { get; init; }
            public string Name { get; init; } = "";
            public string? LookupKey { get; init; }
            public IEnumerable<int> PermissionIds { get; init; } = Enumerable.Empty<int>();
            public IEnumerable<int> PermissionTempIds { get; init; } = Enumerable.Empty<int>();
            public IEnumerable<int> RemovedPermissionIds { get; init; } = Enumerable.Empty<int>();
        }

        [SchemaId(SchemaIds.Boundary.Edit.Input_Permission)]
        public record PermissionObj
        {
            public int TempId { get; init; }
            public int? Id { get; init; }
            public string Name { get; init; } = "";
            public string? LookupKey { get; init; }
        }
    }
}