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

        public async Task<ProjectRole> GetProjectRoleByName(Guid projectId, string role, bool trackChanges)
        {
            return await FindByCondition(r => r.ProjectId.Equals(projectId) && r.Name.Equals(role), trackChanges)
                    .SingleOrDefaultAsync();
        }

        public async Task<ProjectRole> GetProjectRoleById(Guid projectId, Guid roleId, bool trackChanges)
        {
            return await FindByCondition(r => r.ProjectId.Equals(projectId) && r.RoleId.Equals(roleId), trackChanges)
                    .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<ProjectRole>> GetAllProjectRoles(Guid projectId, bool trackChanges)
        {
            return await FindByCondition(r => r.ProjectId.Equals(projectId), trackChanges)
                    .ToListAsync();
        }
    }
}
