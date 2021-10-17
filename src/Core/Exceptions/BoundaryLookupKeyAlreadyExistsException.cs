using System.Runtime.Serialization;
using System.Text;
using CodeCompanion.Exceptions;
using CodeCompanion.FluentEnumerable;

namespace Nova.Identity.Exceptions
{
    public class BoundaryLookupKeyAlreadyExistsException : EntityAlreadyExistsException
    {
        public string LookupKey { get; init; } = "";

        public BoundaryLookupKeyAlreadyExistsException()
        {
        }

        public BoundaryLookupKeyAlreadyExistsException(string? message) : base(message)
        {
        }

        public BoundaryLookupKeyAlreadyExistsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected BoundaryLookupKeyAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        protected override void SetClientMessage(StringBuilder builder) => builder.Append($"Boundary with look-up key '{LookupKey}' already exists");

        protected override void SetErrorData(IDictionary<string, object?> errorData) => errorData.FluentAdd(nameof(LookupKey), LookupKey);
    }
}