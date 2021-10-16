using MediatR;
using Nova.Identity.Responses;

namespace Nova.Identity.Requests
{
    public record AddBoundaryRequest : IRequest<AddBoundaryResponse>
    {
        public string Name { get; init; } = "";
        public string? LookupKey { get; init; }
        public IEnumerable<ClientAppObj> ClientApps { get; init; } = Enumerable.Empty<ClientAppObj>();
        public IEnumerable<RoleObj> Roles { get; init; } = Enumerable.Empty<RoleObj>();
        public IEnumerable<PermissionObj> Permissions { get; init; } = Enumerable.Empty<PermissionObj>();

        public record ClientAppObj
        {
            public string Name { get; init; } = "";
            public string? LookupKey { get; init; }
        }

        public record RoleObj
        {
            public string Name { get; init; } = "";
            public string? LookupKey { get; init; }
            public IEnumerable<int> PermissionTempIds { get; init; } = Enumerable.Empty<int>();
        }

        public record PermissionObj
        {
            public int TempId { get; init; }
            public string Name { get; init; } = "";
            public string? LookupKey { get; init; }
        }
    }
}