using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface ICommentService
    {
        Task<IEnumerable<TaskCommentDto>> GetAllCommentsForTaskAsync(Guid projectId, Guid taskId, bool trackChanges);
        Task AddCommentToTaskAsync(Guid projectId, Guid taskId, TaskCommentForCreationDto commentDto, bool trackChanges);
    }
}
