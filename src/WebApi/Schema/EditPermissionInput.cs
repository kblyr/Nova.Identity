using Nova.Identity.Requests;

namespace Nova.Identity.Schema
{
    [SchemaId(SchemaIds.Permission.Edit.Input)]
    public record EditPermissionInput
    {
        public string Name { get; init; } = "";
        public string? LookupKey { get; init; }
        public short? BoundaryId { get; init; }

        public EditPermissionRequest ToRequest(int id) => new()
        {
            Id = id,
            Name = Name,
            LookupKey = LookupKey,
            BoundaryId = BoundaryId
        };
    }
}