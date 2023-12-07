using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class CardRepository : RepositoryBase<Card>, ICardRepository
    {
        public CardRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Card>> GetAllCardForProjectAsync(Guid projectId, bool trackChanges)
        {
            return await FindAll(trackChanges)
                    .Include(c => c.Group)
                    .Where(c => c.Group.ProjectId.Equals(projectId))
                    .ToListAsync();
        }

        public async Task<IEnumerable<Card>> GetCardsForGroupAsync(Guid groupId, bool trackChanges)
        {
            return await FindByCondition(c => c.GroupId.Equals(groupId), trackChanges)
                    .Include(c => c.Group)
                    .ToListAsync();
        }

        public async Task<Card> GetCardByIdAsync(Guid cardId, bool trackChanges)
        {
            return await FindByCondition(c => c.CardId.Equals(cardId), trackChanges)
                    .Include(c => c.Group)
                    .SingleOrDefaultAsync();
        }

        public void CreateCard(Guid groupId, string creatorId, Card card)
        {
            card.GroupId = groupId;
            card.CreatorId = creatorId;
            card.CreatedAt = DateTime.Now;
            Create(card);
        }

        public void DeleteCard(Card card)
        {
            Delete(card);
        }
    }
}
