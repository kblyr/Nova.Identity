using System.Runtime.Serialization;
using System.Text;
using CodeCompanion.Exceptions;
using CodeCompanion.FluentEnumerable;

namespace Nova.Identity.Exceptions
{
    public class ClientDeviceNameAlreadyExistsException : EntityAlreadyExistsException
    {
        public string Name { get; init; } = "";

        public ClientDeviceNameAlreadyExistsException()
        {
        }

        public ClientDeviceNameAlreadyExistsException(string? message) : base(message)
        {
        }

        public ClientDeviceNameAlreadyExistsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ClientDeviceNameAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        protected override void SetClientMessage(StringBuilder builder) => builder.Append($"Client device with name '{Name}' already exists");

        protected override void SetErrorData(IDictionary<string, object?> errorData) => errorData.FluentAdd(nameof(Name), Name);
    }
}