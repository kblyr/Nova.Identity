using System.Runtime.Serialization;
using System.Text;
using CodeCompanion.Exceptions;
using CodeCompanion.FluentEnumerable;

namespace Nova.Identity.Exceptions
{
    public class ClientDeviceNotFoundException : EntityNotFoundException
    {
        public long Id { get; init; }

        public ClientDeviceNotFoundException()
        {
        }

        public ClientDeviceNotFoundException(string? message) : base(message)
        {
        }

        public ClientDeviceNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ClientDeviceNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        protected override void SetClientMessage(StringBuilder builder) => builder.Append("Client device does not exists");

        protected override void SetErrorData(IDictionary<string, object?> errorData) => errorData.FluentAdd(nameof(Id), Id);
    }
}