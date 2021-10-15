using System.Runtime.Serialization;

namespace Nova.Identity;

class UserNotFoundException : EntityNotFoundException<int>
{
    public UserNotFoundException() { }

    public UserNotFoundException(string? message) : base(message) { }

    public UserNotFoundException(string? message, Exception? innerException) : base(message, innerException) { }

    protected UserNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    protected override string GetClientMessage() => "User does not exists";
}
