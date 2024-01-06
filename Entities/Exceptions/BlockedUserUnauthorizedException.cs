namespace Entities.Exceptions;

public sealed class BlockedUserUnauthorizedException : UnauthorizedException
{
    public BlockedUserUnauthorizedException() : base($"You have been blocked from the application.")
    {
    }
}
