using System.Runtime.Serialization;
using System.Text;
using CodeCompanion.Exceptions;
using CodeCompanion.FluentEnumerable;

namespace Nova.Identity.Exceptions
{
    public class RoleNotFoundException : EntityNotFoundException
    {
        public int Id { get; init; }

        public RoleNotFoundException()
        {
        }

        public RoleNotFoundException(string? message) : base(message)
        {
        }

        public RoleNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected RoleNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        protected override void SetClientMessage(StringBuilder builder) => builder.Append("Role does not exists");

        protected override void SetErrorData(IDictionary<string, object?> errorData) => errorData.FluentAdd(nameof(Id), Id);
    }
}