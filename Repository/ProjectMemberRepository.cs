using Contracts;
using Entities.Models;

namespace Repository
{
    public class ProjectMemberRepository : RepositoryBase<ProjectMember>, IProjectMemberRepository
    {
        public ProjectMemberRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public void AddProjectMember(Guid projectId, string memberId, Guid roleId)
        {
            Create(new ProjectMember
            {
                ProjectId = projectId,
                MemberId = memberId,
                ProjectRoleId = roleId
            });
        }
    }
}
