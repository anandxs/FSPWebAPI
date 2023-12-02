namespace Entities.Exceptions
{
    public sealed class MemberNotFoundException : NotFoundException
    {
        public MemberNotFoundException(string memberId) : base($"Member with id : {memberId} does not exist")
        {
        }
    }
}
