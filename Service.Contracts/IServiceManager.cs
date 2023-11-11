namespace Service.Contracts
{
    public interface IServiceManager
    {
        IProjectService ProjectService { get; }
        IAuthenticationService AuthenticationService { get; }
    }
}
