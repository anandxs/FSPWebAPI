namespace Entities.Exceptions;

public sealed class OwnerCannotBeRemovedBadRequestException : BadRequestException
{
    public OwnerCannotBeRemovedBadRequestException() : base($"Owner cannot exit/be removed from project.")
    {
    }
}
