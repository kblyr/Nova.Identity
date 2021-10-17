using MediatR;
using Nova.Identity.Responses;

namespace Nova.Identity.Requests
{
    public record AddRoleRequest : IRequest<AddRoleResponse>
    {
        public string Name { get; init; } = "";
        public string? LookupKey { get; init; }
        public short? BoundaryId { get; init; }
        public IEnumerable<int> PermissionIds { get; init; } = Enumerable.Empty<int>();
    }
}