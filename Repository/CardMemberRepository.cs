using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class CardMemberRepository : RepositoryBase<CardMember>, ICardMemberRepository
    {
        public CardMemberRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<CardMember>> GetAllCardAssigneesAsync(Guid cardId, bool trackChanges)
        {
            return await FindByCondition(a => a.CardId.Equals(cardId), trackChanges)
                    .Include(a => a.Member)
                    .ToListAsync();
        }

        public async Task<IEnumerable<CardMember>> GetAllAssignedCardsForMemberAsync(Guid memberId, bool trackChanges)
        {
            return await FindByCondition(a => a.MemberId.Equals(memberId), trackChanges)
                    .Include(a => a.Card)
                    .ToListAsync();
        }

        public void AssignMemberToCard(CardMember cardMember)
        {
            Create(cardMember);
        }

        public void UnassignMemberFromCard(CardMember cardMember)
        {
            Delete(cardMember);
        }
    }
}
