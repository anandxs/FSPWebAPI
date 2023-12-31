namespace Contracts
{
    public interface ICommentRepository
    {
        Task<IEnumerable<TaskComment>> GetAllCommentsForTaskAsync(Guid taskId, bool trackChanges);
        void AddCommentToTask(TaskComment comment);
    }
}
