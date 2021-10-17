using System.Runtime.Serialization;
using System.Text;
using CodeCompanion.Exceptions;
using CodeCompanion.FluentEnumerable;

namespace Nova.Identity.Exceptions
{
    public class RolePermissionAlreadyExistsException : EntityAlreadyExistsException
    {
        public RoleObj Role { get; init; } = default!;
        public PermissionObj Permission { get; init; } = default!;

        public RolePermissionAlreadyExistsException()
        {
        }

        public RolePermissionAlreadyExistsException(string? message) : base(message)
        {
        }

        public RolePermissionAlreadyExistsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected RolePermissionAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        protected override void SetClientMessage(StringBuilder builder) => builder.Append($"Role '{Role.Name}' already has permission '{Permission.Name}'");

        protected override void SetErrorData(IDictionary<string, object?> errorData) => errorData
            .FluentAdd(nameof(Role), Role)
            .FluentAdd(nameof(Permission), Permission);

        public record RoleObj
        {
            public int Id { get; init; }
            public string Name { get; init; } = "";
        }

        public record PermissionObj
        {
            public int Id { get; init; }
            public string Name { get; init; } = "";
        }
    }
}