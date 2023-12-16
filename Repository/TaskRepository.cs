using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class TaskRepository : RepositoryBase<ProjectTask>, ITaskRepository
    {
        public TaskRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<ProjectTask>> GetAllTasksForProjectAsync(Guid projectId, bool trackChanges)
        {
            return await FindByCondition(t => t.ProjectId.Equals(projectId), trackChanges)
                        .ToListAsync();
        }

        public async Task<ProjectTask> GetTaskByIdAsync(Guid taskId, bool trackChanges)
        {
            return await FindByCondition(t => t.TaskId.Equals(taskId), trackChanges)
                        .SingleOrDefaultAsync();
        }

        public void CreateTask(ProjectTask task)
        {
            task.IsActive = true;
            task.CreatedAt = DateTime.Now;
            Create(task);
        }

        public void DeleteTask(ProjectTask task)
        {
            Delete(task);
        }
    }
}
