using MediatR;
using Nova.Identity.Responses;

namespace Nova.Identity.Requests
{
    public record AddUserRequest : IRequest<AddUserResponse>
    {
        public string Username { get; init; } = "";
        public string Password { get; init; } = "";
        public bool IsActive { get; init; }
        public bool IsPasswordChangeRequired { get; init; }
        public IEnumerable<short> ClientAppIds { get; init; } = Enumerable.Empty<short>();
        public IEnumerable<int> RoleIds { get; init; } = Enumerable.Empty<int>();
    }
}