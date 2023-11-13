namespace Entities.Exceptions
{
    public sealed class NotAProjectMemberException : ForbiddenRequestException
    {
        public NotAProjectMemberException() : base($"You are not a member of this project.")
        {
        }
    }
}
