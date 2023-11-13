using Entities.Models;

namespace Contracts
{
    public interface IProjectMemberRepository
    {
        void AddProjectMember(Guid projectId, string memberId, Guid roleId);
        Task<ProjectMember> GetProjectMember(Guid projectId, string memberId, bool trackChanges);
    }
}
