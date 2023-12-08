using Entities.Models;

namespace Contracts
{
    public interface IProjectMemberRepository
    {
        Task<IEnumerable<ProjectMember>> GetAllProjectMembersAsync(Guid projectId, bool trackChanges);
        void AddProjectMember(ProjectMember member);
        Task<ProjectMember> GetProjectMemberAsync(Guid projectId, string memberId, bool trackChanges);
        Task<IEnumerable<ProjectMember>> GetProjectsForMemberAsync(string requesterdId, bool trackChanges);
        Task<ProjectMember> GetProjectForMemberAsync(string requesterdId, Guid projectId, bool trackChanges);
        void RemoveMember(ProjectMember projectMember);
    }
}
