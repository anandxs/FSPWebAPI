using Entities.Models;

namespace Contracts
{
    public interface IProjectMemberRepository
    {
        void AddProjectMember(Guid projectId, string memberId, Guid roleId);
        Task<ProjectMember> GetProjectMemberAsync(Guid projectId, string memberId, bool trackChanges);
        Task<IEnumerable<ProjectMember>> GetProjectMembersAsync(Guid projectId, bool trackChanges);
        Task<IEnumerable<ProjectMember>> GetProjectsForMemberAsync(string requesterdId, bool trackChanges);
        Task<ProjectMember> GetProjectForMemberAsync(string requesterdId, Guid projectId, bool trackChanges);
    }
}
