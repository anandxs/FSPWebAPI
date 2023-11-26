namespace Entities.Exceptions
{
    public sealed class NotAProjectMemberForbiddenRequestException : ForbiddenRequestException
    {
        public NotAProjectMemberForbiddenRequestException() : base($"You are not a member of this project.")
        {
        }
    }
}
