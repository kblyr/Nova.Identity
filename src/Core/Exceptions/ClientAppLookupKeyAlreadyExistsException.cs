using System.Runtime.Serialization;
using System.Text;
using CodeCompanion.Exceptions;
using CodeCompanion.FluentEnumerable;

namespace Nova.Identity.Exceptions
{
    public class ClientAppLookupKeyAlreadyExistsException : EntityAlreadyExistsException
    {
        public string LookupKey { get; init; } = "";

        public ClientAppLookupKeyAlreadyExistsException()
        {
        }

        public ClientAppLookupKeyAlreadyExistsException(string? message) : base(message)
        {
        }

        public ClientAppLookupKeyAlreadyExistsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ClientAppLookupKeyAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        protected override void SetClientMessage(StringBuilder builder) => builder.Append($"Client app with look-up key '{LookupKey}' already exists" );

        protected override void SetErrorData(IDictionary<string, object?> errorData) => errorData.FluentAdd(nameof(LookupKey), LookupKey);
    }
}