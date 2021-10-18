using System.Runtime.Serialization;
using System.Text;
using CodeCompanion.Exceptions;
using CodeCompanion.FluentEnumerable;

namespace Nova.Identity.Exceptions
{
    public class RoleNotInBoundaryException : CodeCompanionException
    {
        public RoleObj Role { get; init; } = default!;
        public BoundaryObj Boundary { get; init; } = default!;

        public RoleNotInBoundaryException()
        {
        }

        public RoleNotInBoundaryException(string? message) : base(message)
        {
        }

        public RoleNotInBoundaryException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected RoleNotInBoundaryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        protected override void SetClientMessage(StringBuilder builder) => builder.Append($"Role '{Role.Name}' is not in boundary '{Boundary.Name}'");

        protected override void SetErrorData(IDictionary<string, object?> errorData) => errorData
            .FluentAdd(nameof(Role), Role)
            .FluentAdd(nameof(Boundary), Boundary);

        public record RoleObj
        {
            public int Id { get; init; }
            public string Name { get; init; } = "";
        }

        public record BoundaryObj
        {
            public short Id { get; init; }
            public string Name { get; init; } = "";
        }
    }
}