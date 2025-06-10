namespace MultiType.NET.Core.Exceptions;

public class InvalidUnionStateException: Exception
{
    public InvalidUnionStateException(string message) : base(message) { }
    public InvalidUnionStateException(string message, Exception inner) : base(message, inner) { }
}