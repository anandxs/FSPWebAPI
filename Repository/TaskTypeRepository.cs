using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class TaskTypeRepository : RepositoryBase<TaskType>, ITaskTypeRepository
    {
        public TaskTypeRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<TaskType>> GetAllTaskTypesForProjectAsync(Guid projectId, bool trackChanges)
        {
            return await FindByCondition(t => t.ProjectId.Equals(projectId), trackChanges)
                        .ToListAsync();
        }

        public async Task<TaskType> GetTaskTypeByIdAsync(Guid projectId, Guid typeId, bool trackChanges)
        {
            return await FindByCondition(t => t.ProjectId.Equals(projectId) && t.TypeId.Equals(typeId), trackChanges)
                        .SingleOrDefaultAsync();
        }

        public async Task<TaskType> GetTaskTypeByNameAsync(Guid projectId, string name, bool trackChanges)
        {
            return await FindByCondition(t => t.ProjectId.Equals(projectId) && t.Name.Equals(name), trackChanges)
                        .SingleOrDefaultAsync();
        }

        public void CreateTaskType(TaskType taskType, Guid projectId)
        {
            taskType.ProjectId = projectId;
            Create(taskType);
        }

        public void DeleteTaskType(TaskType taskType)
        {
            Delete(taskType);
        }
    }
}
