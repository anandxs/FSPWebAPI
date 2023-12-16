using Contracts;
using Entities.Models;

namespace Repository
{
    public class CommentRepository : RepositoryBase<TaskComment>, ICommentRepository
    {
        public CommentRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}
