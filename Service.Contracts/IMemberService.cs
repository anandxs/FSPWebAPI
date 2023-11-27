using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface IMemberService
    {
        Task<IEnumerable<ProjectMemberDto>> GetProjectMembersAsync(string userId, Guid projectId, bool trackChanges);
    }
}
