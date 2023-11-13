using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task<ProjectMember> GetProjectMember(Guid projectId, string memberId, bool trackChanges)
        {
            return await FindByCondition(m => m.ProjectId.Equals(projectId) && m.MemberId.Equals(memberId), trackChanges)
                    .SingleOrDefaultAsync();
        }
    }
}
