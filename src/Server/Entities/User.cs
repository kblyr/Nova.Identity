namespace Nova.Identity.Entities
{
    record User
    {
        public int Id { get; init; }
        public string Username { get; init; } = "";
        public string HashedPassword { get; init; } = "";
        public bool IsActive { get; init; }
        public bool IsPasswordChangeRequired { get; init; }
    }
}