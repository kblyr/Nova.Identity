using Nova.Identity.Responses;

namespace Nova.Identity.Schema
{
    [SchemaId(SchemaIds.Role.Edit.Output)]
    public record EditRoleOutput
    {
        public int Id { get; init; }
        public string Name { get; init; } = "";
        public string? LookupKey { get; init; }
        public BoundaryObj? Boundary { get; init; }
        public IEnumerable<PermissionObj> Permissions { get; init; } = Enumerable.Empty<PermissionObj>();

        public static EditRoleOutput From(EditRoleResponse response) => new()
        {
            Id = response.Id,
            Name = response.Name,
            LookupKey = response.LookupKey,
            Boundary = response.Boundary is null ? null : new()
            {
                Id = response.Boundary.Id,
                Name = response.Boundary.Name
            },
            Permissions = response.Permissions.Select(permission => new PermissionObj
            {
                Id = permission.Id,
                Name = permission.Name
            })
        };

        [SchemaId(SchemaIds.Role.Edit.Output_Boundary)]
        public record BoundaryObj
        {
            public short Id { get; init; }
            public string Name { get; init; } = "";
        }

        [SchemaId(SchemaIds.Role.Edit.Output_Permission)]
        public record PermissionObj
        {
            public int Id { get; init; }
            public string Name { get; init; } = "";
        }
    }
}