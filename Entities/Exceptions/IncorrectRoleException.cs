namespace Entities.Exceptions
{
    public sealed class IncorrectRoleException : ForbiddenRequestException
    {
        public IncorrectRoleException() : base($"You do not have rights to this action/resource.")
        {
        }
    }
}
