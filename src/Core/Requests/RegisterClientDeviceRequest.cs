using MediatR;
using Nova.Identity.Responses;

namespace Nova.Identity.Requests
{
    public record RegisterClientDeviceRequest : IRequest<RegisterClientDeviceResponse>
    {
        public long? Id { get; init; }
        public string IpAddress { get; init; } = "";
        public string? Name { get; init; }
    }
}