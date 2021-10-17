using System.Runtime.Serialization;
using System.Text;
using CodeCompanion.Exceptions;
using CodeCompanion.FluentEnumerable;

namespace Nova.Identity.Exceptions
{
    public class RoleLookupKeyAlreadyExistsException : EntityAlreadyExistsException
    {
        public string LookupKey { get; init; } = "";

        public RoleLookupKeyAlreadyExistsException()
        {
        }

        public RoleLookupKeyAlreadyExistsException(string? message) : base(message)
        {
        }

        public RoleLookupKeyAlreadyExistsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected RoleLookupKeyAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        protected override void SetClientMessage(StringBuilder builder) => builder.Append($"Role with look-up key '{LookupKey}' already exists");

        protected override void SetErrorData(IDictionary<string, object?> errorData) => errorData.FluentAdd(nameof(LookupKey), LookupKey);
    }
}