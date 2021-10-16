namespace Nova.Identity.Entities
{
    record RolePermission
    {
        public long Id { get; init; }
        public int RoleId { get; init; }
        public int PermissionId { get; init; }

        public Role Role { get; init; } = default!;
        public Permission Permission { get; init; } = default!;
    }
}