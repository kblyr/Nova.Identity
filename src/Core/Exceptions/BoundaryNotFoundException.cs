using System.Runtime.Serialization;
using System.Text;
using CodeCompanion.Exceptions;
using CodeCompanion.FluentEnumerable;

namespace Nova.Identity.Exceptions
{
    public class BoundaryNotFoundException : EntityNotFoundException
    {
        public short Id { get; init; }

        public BoundaryNotFoundException()
        {
        }

        public BoundaryNotFoundException(string? message) : base(message)
        {
        }

        public BoundaryNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected BoundaryNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        protected override void SetClientMessage(StringBuilder builder) => builder.Append("Boundary does not exists");

        protected override void SetErrorData(IDictionary<string, object?> errorData) => errorData.FluentAdd(nameof(Id), Id);
    }
}