using System.Runtime.Serialization;
using System.Text;
using CodeCompanion.Exceptions;
using CodeCompanion.FluentEnumerable;

namespace Nova.Identity.Exceptions
{
    public class PermissionNotInBoundaryException : CodeCompanionException
    {
        public PermissionObj Permission { get; init; } = default!;
        public BoundaryObj? Boundary { get; init; }

        public PermissionNotInBoundaryException()
        {
        }

        public PermissionNotInBoundaryException(string? message) : base(message)
        {
        }

        public PermissionNotInBoundaryException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected PermissionNotInBoundaryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        protected override void SetClientMessage(StringBuilder builder) => builder.Append
        (
            Boundary is null 
            ? $"Permission '{Permission.Name}' is not a global permission"
            : $"Permission '{Permission.Name}' is not in boundary '{Boundary.Name}'"
        );

        protected override void SetErrorData(IDictionary<string, object?> errorData) => errorData
            .FluentAdd(nameof(Permission), Permission)
            .FluentAdd(nameof(Boundary), Boundary);

        public record PermissionObj
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