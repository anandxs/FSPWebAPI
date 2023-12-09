using Entities.Models;

namespace Contracts
{
    public interface ICardMemberRepository
    {
        Task<IEnumerable<CardMember>> GetAllCardAssigneesAsync(Guid cardId, bool trackChanges);
        Task<IEnumerable<CardMember>> GetAllAssignedCardsForMemberAsync(Guid memberId, bool trackChanges);
        Task<CardMember> GetAssignedMemberAsync(Guid cardId, string memberId, bool trackChanges); 
        void AssignMemberToCard(CardMember cardMember);
        void UnassignMemberFromCard(CardMember cardMember);
    }
}
