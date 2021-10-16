using MediatR;
using Nova.Identity.Responses;

namespace Nova.Identity.Requests
{
    public record AddPermissionRequest : IRequest<AddPermissionResponse>
    {
        public string Name { get; init; } = "";
        public string? LookupKey { get; init; }
    }
}