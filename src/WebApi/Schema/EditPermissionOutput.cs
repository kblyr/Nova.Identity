using Nova.Common.Schema;
using Nova.Identity.Responses;

namespace Nova.Identity.Schema
{
    [SchemaId(SchemaIds.Permission.Edit.Output)]
    public record EditPermissionOutput
    {
        public int Id { get; init; }
        public string Name { get; init; } = "";
        public string? LookupKey { get; init; }
        public BoundaryObj? Boundary { get; init; }

        public static EditPermissionOutput From(EditPermissionResponse response) => new()
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

        [SchemaId(SchemaIds.Permission.Edit.Output_Boundary)]
        public record BoundaryObj
        {
            public short Id { get; init; }
            public string Name { get; init; } = "";
        }
    }
}