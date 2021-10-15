namespace Nova.Identity.Entities;

record UserRole
{
    public long Id { get; init; }
    public int UserId { get; init; }
    public int RoleId { get; init; }
    
    public User User { get; init; } = default!;
    public Role Role { get; init; } = default!;
}