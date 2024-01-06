namespace Entities.Exceptions;

public sealed class RoleNotEmptyBadRequestException : BadRequestException
{
    public RoleNotEmptyBadRequestException(string role) : base($"Members with role {role} exists. Reassign them to an existing role and try again.")
    {
    }
}
