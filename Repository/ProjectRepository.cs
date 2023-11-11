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
            return await FindByCondition(p => p.OwnerId.Equals(userId), trackChanges)
                    .ToListAsync(); ;
        }

        public async Task<Project> GetProjectOwnedByUserAsync(string userId, Guid projectId, bool trackChanges)
        {
            return await FindByCondition(p => p.OwnerId.Equals(userId) && p.ProjectId.Equals(projectId), trackChanges)
                    .SingleOrDefaultAsync();
        }

        public void CreateProject(Project project)
        {
            Create(project);
        }
    }
}
