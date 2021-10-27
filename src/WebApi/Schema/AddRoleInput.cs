using Nova.Identity.Requests;

namespace Nova.Identity.Schema
{
    [SchemaId(SchemaIds.Role.Add.Input)]
    public record AddRoleInput
    {
        public string Name { get; init; } = "";
        public string? LookupKey { get; init; }
        public short? BoundaryId { get; init; }
        public IEnumerable<int> PermissionIds { get; init; } = Enumerable.Empty<int>();

        public AddRoleRequest ToRequest() => new()
        {
            Name = Name,
            LookupKey = LookupKey,
            BoundaryId = BoundaryId,
            PermissionIds = PermissionIds
        };
    }
}