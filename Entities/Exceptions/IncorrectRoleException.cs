namespace Entities.Exceptions
{
    public sealed class IncorrectRoleException : ForbiddenRequestException
    {
        public IncorrectRoleException() : base($"Your role does not have rights to this action.")
        {
        }
    }
}
