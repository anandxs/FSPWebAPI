namespace Entities.Exceptions
{
    public sealed class BlockedUserForbiddenRequest : ForbiddenRequestException
    {
        public BlockedUserForbiddenRequest() : base($"You have been blocked from the application.")
        {
        }
    }
}
