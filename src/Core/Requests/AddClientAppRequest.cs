using MediatR;
using Nova.Identity.Responses;

namespace Nova.Identity.Requests
{
    public record AddClientAppRequest : IRequest<AddClientAppResponse>
    {
        public string Name { get; init; } = "";
        public string? LookupKey { get; init; }
    }
}