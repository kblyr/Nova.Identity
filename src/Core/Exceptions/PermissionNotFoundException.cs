using System.Runtime.Serialization;
using System.Text;
using CodeCompanion.Exceptions;
using CodeCompanion.FluentEnumerable;

namespace Nova.Identity.Exceptions
{
    public class PermissionNotFoundException : EntityNotFoundException
    {
        public int Id { get; init; }
        
        public PermissionNotFoundException()
        {
        }

        public PermissionNotFoundException(string? message) : base(message)
        {
        }

        public PermissionNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected PermissionNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        protected override void SetClientMessage(StringBuilder builder) => builder.Append("Permission does not exists");

        protected override void SetErrorData(IDictionary<string, object?> errorData) => errorData.FluentAdd(nameof(Id), Id);
    }
}