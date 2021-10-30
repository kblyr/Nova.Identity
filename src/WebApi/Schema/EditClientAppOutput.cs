using Nova.Common.Schema;
using Nova.Identity.Responses;

namespace Nova.Identity.Schema
{
    [SchemaId(SchemaIds.ClientApp.Edit.Output)]
    public record EditClientAppOutput
    {
        public short Id { get; init; }
        public string Name { get; init; } = "";
        public string? LookupKey { get; init; }
        public BoundaryObj? Boundary { get; init; }

        public static EditClientAppOutput From(EditClientAppResponse response) => new()
        {
            Id = response.Id,
            Name = response.Name,
            LookupKey = response.LookupKey,
            Boundary = response.Boundary is null ? null : new()
            {
                Id = response.Boundary.Id,
                Name = response.Boundary.Name
            }
        };

        [SchemaId(SchemaIds.ClientApp.Edit.Output_Boundary)]
        public record BoundaryObj
        {
            public short Id { get; init; }
            public string Name { get; init; } = "";
        }
    }
}