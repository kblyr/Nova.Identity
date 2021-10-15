namespace Nova.Identity.Entities;

record UserBoundary
{
    public long Id { get; init; }
    public int UserId { get; init; }
    public int BoundaryId { get; init; }

    public User User { get; init; } = default!;
    public Boundary Boundary { get; init; } = default!;
}