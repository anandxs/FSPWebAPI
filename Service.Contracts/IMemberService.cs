using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface IMemberService
    {
        Task<IEnumerable<ProjectMemberDto>> GetProjectMembersAsync(string userId, Guid projectId, bool trackChanges);
        Task InviteUserAsync(string requesterId, string userId, Guid projectId, MemberForCreationDto memberDto, bool trackChanges);
        Task RemoveMemberAsync(string requesterId, string userId, Guid projectId, string memberId, bool trackChanges);
    }
}
