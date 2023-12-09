using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface ICardMemberService
    {
        Task<IEnumerable<CardDto>> GetAllCardAssigneesAsync(Guid projectId, Guid cardId, bool trackChanges);
    }
}
