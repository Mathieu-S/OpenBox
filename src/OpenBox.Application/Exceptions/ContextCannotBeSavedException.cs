using System.Runtime.Serialization;

namespace OpenBox.Application.Exceptions;

public class ContextCannotBeSavedException : Exception
{
    public ContextCannotBeSavedException()
    {
    }

    public ContextCannotBeSavedException(string message) : base(message)
    {
    }

    public ContextCannotBeSavedException(string message, Exception inner) : base(message, inner)
    {
    }

    protected ContextCannotBeSavedException(SerializationInfo serializationInfo, StreamingContext streamingContext) :
        base(serializationInfo, streamingContext)
    {
    }
}