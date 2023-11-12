namespace Service.Contracts
{
    public interface IServiceManager
    {
        IProjectService ProjectService { get; }
        IGroupService GroupService { get; }
        IAuthenticationService AuthenticationService { get; }
    }
}
