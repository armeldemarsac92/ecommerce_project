namespace Tdev702.Contracts.Exceptions;

public class EmptyCodeException : Exception
{
    public EmptyCodeException(string message) : base(message) { }
}