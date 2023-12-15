using Contracts;
using Entities.Models;

namespace Repository
{
    public class TagRepository : RepositoryBase<Tag>, ITagRepository
    {
        public TagRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}
