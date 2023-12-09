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
                    .Include(a => a.Card)
                    .ToListAsync();
        }

        public async Task<IEnumerable<CardMember>> GetAllAssignedCardsForMemberAsync(string memberId, bool trackChanges)
        {
            return await FindByCondition(a => a.MemberId.Equals(memberId), trackChanges)
                    .Include(a => a.Card)
                    .Include(a => a.Member)
                    .ToListAsync();
        }

        public async Task<CardMember> GetAssignedMemberAsync(Guid cardId, string memberId, bool trackChanges)
        {
            return await FindByCondition(m => m.CardId.Equals(cardId) && m.MemberId.Equals(memberId), trackChanges)
                    .SingleOrDefaultAsync();
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
