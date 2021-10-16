namespace Nova.Identity.Entities
{
    record UserClientApp
    {
        public long Id { get; init; }
        public int UserId { get; init; }
        public short ClientAppId { get; init; }

        public User User { get; init; } = default!;
        public ClientApp ClientApp { get; init; } = default!;
    }
}