using System.Runtime.Serialization;

namespace Nova.Identity;

class ClientAppNotFoundException : EntityNotFoundException<int>
{
    public ClientAppNotFoundException() { }

    public ClientAppNotFoundException(string? message) : base(message) { }

    public ClientAppNotFoundException(string? message, Exception? innerException) : base(message, innerException) { }

    protected ClientAppNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    protected override string GetClientMessage() => "Client application does not exists";
}
