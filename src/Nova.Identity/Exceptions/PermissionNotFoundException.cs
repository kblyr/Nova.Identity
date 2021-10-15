using System.Runtime.Serialization;

namespace Nova.Identity;

class PermissionNotFoundException : EntityNotFoundException<int>
{
    public PermissionNotFoundException() { }

    public PermissionNotFoundException(string? message) : base(message) { }

    public PermissionNotFoundException(string? message, Exception? innerException) : base(message, innerException) { }

    protected PermissionNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}