using Contracts;
using Entities.Models;

namespace Repository
{
    public class TaskTypeRepository : RepositoryBase<TaskType>, ITaskTypeRepository
    {
        public TaskTypeRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}
