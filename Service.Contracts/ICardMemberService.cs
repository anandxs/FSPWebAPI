using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface ICardMemberService
    {
        Task<IEnumerable<UserDto>> GetAllCardAssigneesAsync(Guid projectId, Guid cardId, bool trackChanges);
        Task AssignMemberToCardAsync(Guid projectId, Guid cardId, string memberId, bool trackChanges);
        Task UnassignMemberFromCardAsync(Guid projectid, Guid cardId, string memberId, bool trackChanges);
    }
}
