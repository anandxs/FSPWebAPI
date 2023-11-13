namespace Service.Contracts
{
    public interface IServiceManager
    {
        IProjectService ProjectService { get; }
        IGroupService GroupService { get; }
        ICardService CardService { get; }
        IAuthenticationService AuthenticationService { get; }
    }
}
