using System.Runtime.Serialization;

namespace Nova.Identity;

class BoundaryNotFoundException : EntityNotFoundException<int>
{
    public BoundaryNotFoundException() { }

    public BoundaryNotFoundException(string? message) : base(message) { }

    public BoundaryNotFoundException(string? message, Exception? innerException) : base(message, innerException) { }

    protected BoundaryNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    protected override string GetClientMessage() => "Boundary does not exists";
}