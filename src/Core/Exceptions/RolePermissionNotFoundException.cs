using System.Runtime.Serialization;
using System.Text;
using CodeCompanion.Exceptions;
using CodeCompanion.FluentEnumerable;

namespace Nova.Identity.Exceptions
{
    public class RolePermissionNotFoundException : EntityNotFoundException
    {
        public RoleObj Role { get; init; } = default!;
        public PermissionObj Permission { get; init; } = default!;

        public RolePermissionNotFoundException()
        {
        }

        public RolePermissionNotFoundException(string? message) : base(message)
        {
        }

        public RolePermissionNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected RolePermissionNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        protected override void SetClientMessage(StringBuilder builder) => builder.Append($"Role '{Role.Name}' has no permission '{Permission.Name}'");

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