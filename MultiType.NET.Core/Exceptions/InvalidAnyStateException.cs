namespace MultiType.NET.Core.Exceptions;

public class InvalidAnyStateException: Exception
{
    public InvalidAnyStateException(string message) : base(message) { }
    public InvalidAnyStateException(string message, Exception inner) : base(message, inner) { }
}