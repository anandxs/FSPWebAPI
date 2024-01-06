namespace Entities.Exceptions;

public sealed class BlockedUserForbiddenException : ForbiddenRequestException
{
    public BlockedUserForbiddenException() : base("You have been blocked from the application")
    {
    }
}
