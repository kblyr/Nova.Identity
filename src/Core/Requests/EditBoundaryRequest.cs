using MediatR;
using Nova.Identity.Responses;

namespace Nova.Identity.Requests
{
    public record EditBoundaryRequest : IRequest<EditBoundaryResponse>
    {
        public short Id { get; init; }
        public string Name { get; init; } = "";
        public string? LookupKey { get; init; }
        public IEnumerable<ClientAppObj> ClientApps { get; init; } = Enumerable.Empty<ClientAppObj>();
        public IEnumerable<RoleObj> Roles { get; init; } = Enumerable.Empty<RoleObj>();
        public IEnumerable<PermissionObj> Permissions { get; init; } = Enumerable.Empty<PermissionObj>();
        public IEnumerable<short> DeletedClientAppIds { get; init; } = Enumerable.Empty<short>();
        public IEnumerable<int> DeletedRoleIds { get; init; } = Enumerable.Empty<int>();
        public IEnumerable<int> DeletedPermissionIds { get; init; } = Enumerable.Empty<int>();

        public record ClientAppObj
        {
            public short? Id { get; init; }
            public string Name { get; init; } = "";
            public string? LookupKey { get; init; }
        }

        public record RoleObj
        {
            public int? Id { get; init; }
            public string Name { get; init; } = "";
            public string? LookupKey { get; init; }
            public IEnumerable<int> PermissionIds { get; init; } = Enumerable.Empty<int>();
            public IEnumerable<int> PermissionTempIds { get; init; } = Enumerable.Empty<int>();
            public IEnumerable<int> RemovedPermissionIds { get; init; } = Enumerable.Empty<int>();
        }

        public record PermissionObj
        {
            public int TempId { get; init; }
            public int? Id { get; init; }
            public string Name { get; init; } = "";
            public string? LookupKey { get; init; }
        }
    }
}