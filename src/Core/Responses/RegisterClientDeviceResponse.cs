namespace Nova.Identity.Responses
{
    public record RegisterClientDeviceResponse
    {
        public long Id { get; init; }
        public string IpAddress { get; init; } = "";
        public string Name { get; init; } = "";
    }
}