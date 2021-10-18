using System.Runtime.Serialization;
using System.Text;
using CodeCompanion.Exceptions;
using CodeCompanion.FluentEnumerable;

namespace Nova.Identity.Exceptions
{
    public class ClientAppNotInBoundaryException : CodeCompanionException
    {
        public ClientAppObj ClientApp { get; init; } = default!;
        public BoundaryObj Boundary { get; init; } = default!;

        public ClientAppNotInBoundaryException()
        {
        }

        public ClientAppNotInBoundaryException(string? message) : base(message)
        {
        }

        public ClientAppNotInBoundaryException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ClientAppNotInBoundaryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        protected override void SetClientMessage(StringBuilder builder) => builder.Append($"Client app '{ClientApp.Name}' is not in boundary '{Boundary.Name}'");

        protected override void SetErrorData(IDictionary<string, object?> errorData) => errorData
            .FluentAdd(nameof(ClientApp), ClientApp)
            .FluentAdd(nameof(Boundary), Boundary);

        public record ClientAppObj
        {
            public short Id { get; init; }
            public string Name { get; init; } = "";
        }

        public record BoundaryObj
        {
            public short Id { get; init; }
            public string Name { get; init; } = "";
        }
    }
}