using System.Runtime.Serialization;

namespace Nova.Identity;

class NovaException : Exception
{
    public const string DefaultClientMessage = "An error occured";

    public string ClientMessage => GetClientMessage();
    public IDictionary<string, object?> ErrorData 
    {
        get
        {
            var errorData = new Dictionary<string, object?>();
            SetErrorData(errorData);
            return errorData;
        }
    }

    public NovaException() { }

    public NovaException(string? message) : base(message) { }

    public NovaException(string? message, Exception? innerException) : base(message, innerException) { }

    protected NovaException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    protected virtual string GetClientMessage() => DefaultClientMessage;

    protected virtual void SetErrorData(IDictionary<string, object?> errorData) { }
}