namespace Entities.Exceptions;

public sealed class MemberAlreadyExistsBadRequest : BadRequestException
{
    public MemberAlreadyExistsBadRequest(string email) : base($"User with email : {email} already exists.")
    {
    }
}
