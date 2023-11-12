using Contracts;
using Entities.Models;

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
    }
}
