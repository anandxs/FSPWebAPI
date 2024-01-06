namespace Entities.Exceptions;

public sealed class MemberAlreadyAssignedToCardBadRequestException : BadRequestException
{
    public MemberAlreadyAssignedToCardBadRequestException() : base($"Member is already assigned to this card.")
    {
    }
}
