namespace Entities.Exceptions;

public abstract class ForbiddenRequestException : Exception
{
    protected ForbiddenRequestException(string message) : base(message)
    {
    }
}
