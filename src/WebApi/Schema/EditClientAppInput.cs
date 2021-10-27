using Nova.Identity.Requests;

namespace Nova.Identity.Schema
{
    [SchemaId(SchemaIds.ClientApp.Edit.Input)]
    public record EditClientAppInput
    {
        public string Name { get; init; } = "";
        public string? LookupKey { get; init; }
        public short? BoundaryId { get; init; }

        public EditClientAppRequest ToRequest(short id) => new()
        {
            Id = id,
            Name = Name,
            LookupKey = LookupKey,
            BoundaryId = BoundaryId
        };
    }
}