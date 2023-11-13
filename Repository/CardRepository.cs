using Contracts;
using Entities.Models;

namespace Repository
{
    public class CardRepository : RepositoryBase<Card>, ICardRepository
    {
        public CardRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}
