using Nova.Common.Schema;
using Nova.Identity.Requests;

namespace Nova.Identity.Schema
{
    [SchemaId(SchemaIds.ClientApp.Add.Input)]
    public record AddClientAppInput 
    {
        public string Name { get; init; } = "";
        public string? LookupKey { get; init; }
        public short? BoundaryId { get; init; }

        public AddClientAppRequest ToRequest() => new()
        {
            Name = Name,
            LookupKey = LookupKey,
            BoundaryId = BoundaryId
        };
    }
}