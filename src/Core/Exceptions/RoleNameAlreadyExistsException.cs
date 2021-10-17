using System.Runtime.Serialization;
using System.Text;
using CodeCompanion.Exceptions;
using CodeCompanion.FluentEnumerable;

namespace Nova.Identity.Exceptions
{
    public class RoleNameAlreadyExistsException : EntityAlreadyExistsException
    {
        public string Name { get; init; } = "";
        public BoundaryObj? Boundary { get; init; }

        public RoleNameAlreadyExistsException()
        {
        }

        public RoleNameAlreadyExistsException(string? message) : base(message)
        {
        }

        public RoleNameAlreadyExistsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected RoleNameAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        protected override void SetClientMessage(StringBuilder builder) => builder.Append(
            Boundary is null
                ? $"Role with name '{Name}' already exists" 
                : $"Role with name '{Name}' in boundary '{Boundary.Name}' already exists"
        );

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