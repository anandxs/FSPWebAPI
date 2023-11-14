using Entities.Models;

namespace Contracts
{
    public interface ICardRepository
    {
        Task<IEnumerable<Card>> GetCardsForGroupAsync(Guid groupId, bool trackChanges);
        Task<Card> GetCardByIdAsync(Guid groupId, Guid cardId, bool trackChanges);
        void CreateCard(Guid groupId, string creatorId, Card card);
        void DeleteCard(Card card);
    }
}
