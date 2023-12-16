using Contracts;
using Entities.Models;

namespace Repository
{
    public class TaskRepository : RepositoryBase<ProjectTask>, ITaskRepository
    {
        public TaskRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}
