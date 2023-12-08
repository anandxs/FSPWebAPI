using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface IMemberService
    {
        Task<IEnumerable<ProjectMemberDto>> GetAllProjectMembersAsync(Guid projectId, string requesterId, bool trackChanges);
        Task AddMemberAsync(Guid projectId, string requesterId, MemberForCreationDto memberDto, bool trackChanges);
        Task RemoveMemberAsync(Guid projectId, string memberId, string requesterId, bool trackChanges);
    }
}
