namespace Repository
{
    public class UserInviteRepository : RepositoryBase<UserInvite>, IUserInviteRepository
    {
        public UserInviteRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<UserInvite>> GetAllUserInvitesAsync(string email, bool trackChanges)
        {
            return await FindByCondition(x => x.Email.Equals(email) && x.Status.Equals(Constants.PROJECT_INVITE_INVITED), trackChanges)
                        .ToListAsync();
        }

        public async Task<UserInvite> GetUserInviteAsync(Guid projectId, string email, bool trackChanges)
        {
            return await FindByCondition(x => x.ProjectId.Equals(projectId) && x.Email.Equals(email), trackChanges)
                        .SingleOrDefaultAsync();
        }

        public void AddToInviteList(UserInvite entity)
        {
            Create(entity);
        }
    }
}
