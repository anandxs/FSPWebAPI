namespace Repository
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<User>> GetAllUsers(bool trackChanges)
        {
            return await FindAll(trackChanges)
                        .Where(x => !x.Email.Equals("admin@mail.com"))
                        .ToListAsync();
        }
    }
}
