namespace Contracts
{
    public interface IRepositoryManager
    {
        IProjectRepository ProjectRepository { get; }
        IRoleRepository RoleRepository { get; }
        IStageRepository StageRepository { get; }
        ITaskTypeRepository TaskTypeRepository { get; }
        IProjectMemberRepository ProjectMemberRepository { get; }
        ICardRepository CardRepository { get; }
        ICardMemberRepository CardMemberRepository { get; }
        IUserRepository UserRepository { get; }
        Task SaveAsync();
    }
}
