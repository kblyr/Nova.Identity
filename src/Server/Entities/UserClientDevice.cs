namespace Nova.Identity.Entities
{
    record UserClientDevice
    {
        public long Id { get; init; }
        public int UserId { get; init; }
        public long ClientDeviceId { get; init; }

        public User User { get; init; } = default!;
        public ClientDevice ClientDevice { get; init; } = default!;
    }
}