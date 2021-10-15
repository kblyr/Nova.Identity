using System.Runtime.Serialization;

namespace Nova.Identity;

class EntityNotFoundException<TId> : NovaException
{
    public TId Id { get; init; } = default!;

    public EntityNotFoundException() { }

    public EntityNotFoundException(string? message) : base(message) { }

    public EntityNotFoundException(string? message, Exception? innerException) : base(message, innerException) { }

    protected EntityNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    protected override string GetClientMessage() => "Entity does not exists";

    protected override void SetErrorData(IDictionary<string, object?> errorData) => errorData.FluentAdd(nameof(Id), Id);
}