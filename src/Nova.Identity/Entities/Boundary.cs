namespace Nova.Identity.Entities;

record Boundary
{
    public int Id { get; init; }
    public string Name { get; init; } = "";
    public string? LookupKey { get; init; }
}