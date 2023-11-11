using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class ProjectRepository : RepositoryBase<Project>, IProjectRepository
    {
        public ProjectRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Project>> GetProjectsOwnedByUserAsync(string userId, bool trackChanges)
        {
            return await FindByCondition(x => x.OwnerId.Equals(userId), trackChanges)
                    .ToListAsync(); ;
        }
    }
}
