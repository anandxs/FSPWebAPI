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
        IAttachmentRepository AttachmentRepository { get; }
        IProjectMemberRepository ProjectMemberRepository { get; }
        IUserInviteRepository UserInviteRepository { get; }
        IUserRepository UserRepository { get; }
        IChatRepository ChatRepository { get; }
        Task SaveAsync();
    }
}
