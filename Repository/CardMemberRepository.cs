using Contracts;
using Entities.Models;

namespace Repository
{
    public class CardMemberRepository : RepositoryBase<CardMember>, ICardMemberRepository
    {
        public CardMemberRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}
