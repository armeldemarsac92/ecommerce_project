namespace Tdev702.Contracts.Exceptions;

public class DatabaseException : Exception
{
    public DatabaseException(string message, Exception innerException) : base(message, innerException)
    {
    }
}