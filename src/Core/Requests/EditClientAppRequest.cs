using MediatR;
using Nova.Identity.Responses;

namespace Nova.Identity.Requests
{
    public record EditClientAppRequest : IRequest<EditClientAppResponse>
    {
        public short Id { get; init; }
        public string Name { get; init; } = "";
        public string? LookupKey { get; init; }
        public short? BoundaryId { get; init; }
    }
}