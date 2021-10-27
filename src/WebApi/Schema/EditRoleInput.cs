using Nova.Identity.Requests;

namespace Nova.Identity.Schema
{
    [SchemaId(SchemaIds.Role.Edit.Input)]
    public record EditRoleInput
    {
        public string Name { get; init; } = "";
        public string? LookupKey { get; init; }
        public short? BoundaryId { get; init; }
        public IEnumerable<int> PermissionIds { get; init; } = Enumerable.Empty<int>();
        public IEnumerable<int> RemovedPermissionIds { get; init; } = Enumerable.Empty<int>();

        public EditRoleRequest ToRequest(int id) => new()
        {
            Id = id,
            Name = Name,
            LookupKey = LookupKey,
            BoundaryId = BoundaryId,
            PermissionIds = PermissionIds,
            RemovedPermissionIds = RemovedPermissionIds
        };
    }
}