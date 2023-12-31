namespace Contracts
{
    public interface IUserInviteRepository
    {
        Task<UserInvite> GetUserInviteAsync(Guid projectId, string email, bool trackChanges);
        Task<IEnumerable<UserInvite>> GetAllUserInvitesAsync(string email, bool trackChanges);
        void AddToInviteList(UserInvite entity);
    }
}
