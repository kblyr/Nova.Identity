using System.Runtime.Serialization;
using System.Text;
using CodeCompanion.Exceptions;
using CodeCompanion.FluentEnumerable;

namespace Nova.Identity.Exceptions
{
    public class ClientAppNameAlreadyExistsException : EntityAlreadyExistsException
    {
        public string Name { get; init; } = "";
        public BoundaryObj? Boundary { get; init; }

        public ClientAppNameAlreadyExistsException()
        {
        }

        public ClientAppNameAlreadyExistsException(string? message) : base(message)
        {
        }

        public ClientAppNameAlreadyExistsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ClientAppNameAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        protected override void SetClientMessage(StringBuilder builder)
        {
            if (Boundary is not null)
                builder.Append($"Client app with name '{Name}' in boundary '{Boundary.Name}' already exists");
            else
                builder.Append($"Client app with name '{Name}' already exists");
        }

        protected override void SetErrorData(IDictionary<string, object?> errorData) => errorData
            .FluentAdd(nameof(Name), Name)
            .FluentAddIf(nameof(Boundary), Boundary, Boundary is not null);

        public record BoundaryObj
        {
            public short Id { get; init; }
            public string Name { get; init; } = "";
        }
    }
}