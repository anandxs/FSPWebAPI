namespace Contracts
{
    public interface IRepositoryManager
    {
        IProjectRepository ProjectRepository { get; }
        IGroupRepository GroupRepository { get; }
        IProjectRoleRepository ProjectRoleRepository { get; }
        Task SaveAsync();
    }
}
