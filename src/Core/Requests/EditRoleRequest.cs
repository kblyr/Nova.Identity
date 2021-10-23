using MediatR;
using Nova.Identity.Responses;

namespace Nova.Identity.Requests
{
    public record EditRoleRequest : IRequest<EditRoleResponse>
    {
        public int Id { get; init; }
        public string Name { get; init; } = "";
        public string? LookupKey { get; init; }
        public short? BoundaryId { get; init; }
        public IEnumerable<int> PermissionIds { get; init; } = Enumerable.Empty<int>();
        public IEnumerable<int> RemovedPermissionIds { get; init; } = Enumerable.Empty<int>();
    }
}