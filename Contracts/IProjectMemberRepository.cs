using Entities.Models;

namespace Contracts
{
    public interface IProjectMemberRepository
    {
        void AddProjectMember(Guid projectId, string memberId, Guid roleId);
        Task<ProjectMember> GetProjectMemberAsync(Guid projectId, string memberId, bool trackChanges);
        Task<IEnumerable<ProjectMember>> GetProjectMembersAsync(Guid projectId, bool trackChanges);
    }
}
