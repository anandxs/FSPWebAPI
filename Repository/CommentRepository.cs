namespace Repository;

public class CommentRepository : RepositoryBase<TaskComment>, ICommentRepository
{
    public CommentRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }

    public async Task<IEnumerable<TaskComment>> GetAllCommentsForTaskAsync(Guid taskId, bool trackChanges)
    {
        return await FindByCondition(c => c.TaskId.Equals(taskId), trackChanges)
                    .Include(c => c.Commenter)
                    .ToListAsync();
    }

    public void AddCommentToTask(TaskComment comment)
    {
        comment.CreatedAt = DateTime.Now;
        Create(comment);
    }
}
