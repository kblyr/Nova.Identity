using System.Runtime.Serialization;

namespace Nova.Identity;

class RoleNotFoundException : EntityNotFoundException<int>
{
    public RoleNotFoundException() { }

    public RoleNotFoundException(string? message) : base(message) { }

    public RoleNotFoundException(string? message, Exception? innerException) : base(message, innerException) { }

    protected RoleNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    protected override string GetClientMessage() => "Role does not exists";
}