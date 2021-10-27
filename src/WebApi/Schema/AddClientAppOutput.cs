using Nova.Identity.Responses;

namespace Nova.Identity.Schema
{
    [SchemaId(SchemaIds.ClientApp.Add.Output)]
    public record AddClientAppOutput
    {
        public short Id { get; init; }
        public string Name { get; init; } = "";
        public string? LookupKey { get; init; }
        public BoundaryObj? Boundary { get; init; }

        public static AddClientAppOutput From(AddClientAppResponse response) => new()
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
        
        [SchemaId(SchemaIds.ClientApp.Add.Output_Boundary)]
        public record BoundaryObj
        {
            public short Id { get; init; }
            public string Name { get; init; } = "";
        }
    }
}