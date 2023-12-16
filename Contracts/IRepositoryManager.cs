namespace Contracts
{
    public interface IRepositoryManager
    {
        IProjectRepository ProjectRepository { get; }
        IRoleRepository RoleRepository { get; }
        IStageRepository StageRepository { get; }
        ITaskTypeRepository TaskTypeRepository { get; }
        ITagRepository TagRepository { get; }
        ITaskRepository TaskRepository { get; }
        ICommentRepository CommentRepository { get; }
        IProjectMemberRepository ProjectMemberRepository { get; }
        IUserRepository UserRepository { get; }
        Task SaveAsync();
    }
}
