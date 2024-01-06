namespace Entities.Exceptions;

public sealed class EmailNotConfirmedUnauthorizedException : UnauthorizedException
{
    public EmailNotConfirmedUnauthorizedException(string email) : base($"The email {email} has not been confirmed.")
    {
    }
}
