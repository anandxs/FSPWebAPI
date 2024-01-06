namespace Entities.Exceptions;

public sealed class IncorrectRoleForbiddenRequestException : ForbiddenRequestException
{
    public IncorrectRoleForbiddenRequestException() : base($"You do not have rights to this action/resource.")
    {
    }
}
