﻿using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace Repository
{
    public class CardRepository : RepositoryBase<Card>, ICardRepository
    {
        public CardRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public void CreateCard(Guid groupId, string creatorId, Card card)
        {
            card.GroupId = groupId;
            card.CreatorId = creatorId;
            card.CreatedAt = DateTime.Now;
            Create(card);
        }

        public async Task<Card> GetCardByIdAsync(Guid groupId, Guid cardId, bool trackChanges)
        {
            return await FindByCondition(c => c.CardId.Equals(cardId), trackChanges).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Card>> GetCardsForGroupAsync(Guid groupId, bool trackChanges)
        {
            return await FindByCondition(c => c.GroupId.Equals(groupId), trackChanges).ToListAsync();
        }

        public void DeleteCard(Card card)
        {
            Delete(card);
        }
    }
}