using System.Runtime.Serialization;
using System.Text;
using CodeCompanion.Exceptions;
using CodeCompanion.FluentEnumerable;

namespace Nova.Identity.Exceptions
{
    public class ClientAppNotFoundException : EntityNotFoundException
    {
        public short Id { get; init; }

        public ClientAppNotFoundException()
        {
        }

        public ClientAppNotFoundException(string? message) : base(message)
        {
        }

        public ClientAppNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ClientAppNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        protected override void SetClientMessage(StringBuilder builder) => builder.Append("Client app does not exists");

        protected override void SetErrorData(IDictionary<string, object?> errorData) => errorData.FluentAdd(nameof(Id), Id);
    }
}