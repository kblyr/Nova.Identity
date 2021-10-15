namespace Nova.Identity.Entities;

record ClientApp
{
    public int Id { get; init; }
    public string Name { get; init; } = "";
    public string? LookupKey { get; init; }
    public int? BoundaryId { get; init; }

    public Boundary? Boundary { get; init; } 
}