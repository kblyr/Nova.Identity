using System.Runtime.Serialization;
using System.Text;
using CodeCompanion.Exceptions;
using CodeCompanion.FluentEnumerable;

namespace Nova.Identity.Exceptions
{
    public class ClientDeviceIpAddressAlreadyExistsException : EntityAlreadyExistsException
    {
        public string IpAddress { get; init; } = "";

        public ClientDeviceIpAddressAlreadyExistsException()
        {
        }

        public ClientDeviceIpAddressAlreadyExistsException(string? message) : base(message)
        {
        }

        public ClientDeviceIpAddressAlreadyExistsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ClientDeviceIpAddressAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        protected override void SetClientMessage(StringBuilder builder) => builder.Append($"Client device with ip address '{IpAddress}' already exists");

        protected override void SetErrorData(IDictionary<string, object?> errorData) => errorData.FluentAdd(nameof(IpAddress), IpAddress);
    }
}