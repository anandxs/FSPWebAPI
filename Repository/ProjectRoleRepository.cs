using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class ProjectRoleRepository : RepositoryBase<ProjectRole>, IProjectRoleRepository
    {
        public ProjectRoleRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public void DefaultProjectRoleCreation(Guid projectId)
        {
            Create(new ProjectRole
            {
                Name = "Admin",
                ProjectId = projectId
            });

            Create(new ProjectRole
            {
                Name = "Member",
                ProjectId = projectId
            });

            Create(new ProjectRole
            {
                Name = "Observer",
                ProjectId = projectId
            });
        }

        public async Task<ProjectRole> GetProjectRole(Guid projectId, string role, bool trackChanges)
        {
            return await FindByCondition(r => r.ProjectId.Equals(projectId) && r.Name.Equals(role), trackChanges).SingleOrDefaultAsync();
        }
    }
}
