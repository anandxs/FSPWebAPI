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

        public async Task<ProjectMember> GetProjectMemberAsync(Guid projectId, string memberId, bool trackChanges)
        {
            return await FindByCondition(m => m.ProjectId.Equals(projectId) && m.MemberId.Equals(memberId), trackChanges)
                    .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<ProjectMember>> GetProjectMembersAsync(Guid projectId, bool trackChanges)
        {
            return await FindByCondition(m => m.ProjectId.Equals(projectId), trackChanges)
                    .Include(m => m.User)
                    .Include(m => m.ProjectRole)
                    .ToListAsync();
        }

        public async Task<IEnumerable<ProjectMember>> GetProjectsForMemberAsync(string requesterdId, bool trackChanges)
        {
            return await FindByCondition(m => m.MemberId.Equals(requesterdId), trackChanges)
                    .Include(m => m.Project)
                    .ToListAsync();
        }

        public async Task<ProjectMember> GetProjectForMemberAsync(string requesterdId, Guid projectId,bool trackChanges)
        {
            return await FindByCondition(m => m.MemberId.Equals(requesterdId) && m.ProjectId.Equals(projectId), trackChanges)
                    .Include(m => m.Project)
                    .SingleOrDefaultAsync();
        }

        public void RemoveMember(ProjectMember projectMember)
        {
            Delete(projectMember);
        }
    }
}
