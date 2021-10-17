using System.Runtime.Serialization;
using System.Text;
using CodeCompanion.Exceptions;
using CodeCompanion.FluentEnumerable;

namespace Nova.Identity.Exceptions
{
    public class BoundaryNameAlreadyExistsException : EntityAlreadyExistsException
    {
        public string Name { get; init; } = "";

        public BoundaryNameAlreadyExistsException()
        {
        }

        public BoundaryNameAlreadyExistsException(string? message) : base(message)
        {
        }

        public BoundaryNameAlreadyExistsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected BoundaryNameAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        protected override void SetClientMessage(StringBuilder builder) => builder.Append($"Boundary with name '{Name}' already exists");

        protected override void SetErrorData(IDictionary<string, object?> errorData) => errorData.FluentAdd(nameof(Name), Name);
    }
}