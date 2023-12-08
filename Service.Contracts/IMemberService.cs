using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface IMemberService
    {
        Task AddMemberAsync(Guid projectId, string requesterId, MemberForCreationDto memberDto, bool trackChanges);
        Task RemoveMemberAsync(string requesterId, string userId, Guid projectId, string memberId, bool trackChanges);
    }
}
