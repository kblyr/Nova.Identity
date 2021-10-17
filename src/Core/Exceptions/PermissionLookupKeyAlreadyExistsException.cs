using System.Runtime.Serialization;
using System.Text;
using CodeCompanion.Exceptions;
using CodeCompanion.FluentEnumerable;

namespace Nova.Identity.Exceptions
{
    public class PermissionLookupKeyAlreadyExistsException : EntityAlreadyExistsException
    {
        public string LookupKey { get; init; } = "";
        public BoundaryObj? Boundary { get; init; }

        public PermissionLookupKeyAlreadyExistsException()
        {
        }

        public PermissionLookupKeyAlreadyExistsException(string? message) : base(message)
        {
        }

        public PermissionLookupKeyAlreadyExistsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected PermissionLookupKeyAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        protected override void SetClientMessage(StringBuilder builder) => builder.Append(
            Boundary is null
                ? $"Permission with look-up key '{LookupKey}' already exists" 
                : $"Permission with look-up key '{LookupKey}' in boundary '{Boundary.Name}' already exists"
        );

        protected override void SetErrorData(IDictionary<string, object?> errorData) => errorData
            .FluentAdd(nameof(LookupKey), LookupKey)
            .FluentAddIf(nameof(Boundary), Boundary, Boundary is not null);

        public record BoundaryObj
        {
            public short Id { get; init; }
            public string Name { get; init; } = "";
        }
    }
}