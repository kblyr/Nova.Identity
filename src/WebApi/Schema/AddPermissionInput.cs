using Nova.Identity.Requests;

namespace Nova.Identity.Schema
{
    [SchemaId(SchemaIds.Permission.Add.Input)]
    public record AddPermissionInput
    {
        public string Name { get; init; } = "";
        public string? LookupKey { get; init; }
        public short? BoundaryId { get; init; }

        public AddPermissionRequest ToRequest() => new()
        {
            Name = Name,
            LookupKey = LookupKey,
            BoundaryId = BoundaryId
        };
    }
}