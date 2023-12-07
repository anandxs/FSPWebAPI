using Entities.Models;

namespace Contracts
{
    public interface ICardRepository
    {
        Task<IEnumerable<Card>> GetAllCardForProjectAsync(Guid projectId, bool trackChanges);
        Task<IEnumerable<Card>> GetCardsForGroupAsync(Guid groupId, bool trackChanges);
        Task<Card> GetCardByIdAsync(Guid cardId, bool trackChanges);
        void CreateCard(Guid groupId, string creatorId, Card card);
        void DeleteCard(Card card);
    }
}
