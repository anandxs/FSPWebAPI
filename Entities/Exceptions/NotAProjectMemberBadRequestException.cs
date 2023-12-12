namespace Entities.Exceptions
{
    public sealed class NotAProjectMemberBadRequestException : BadRequestException
    {
        public NotAProjectMemberBadRequestException(string id) : base($"User with id : {id} is not a member of project.")
        {
        }
    }
}
