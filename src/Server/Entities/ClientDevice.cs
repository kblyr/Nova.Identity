namespace Nova.Identity.Entities
{
    record ClientDevice
    {
        public long Id { get; init; }
        public string IpAddress { get; init; } = "";
        public string Name { get; init; } = "";
    }
}