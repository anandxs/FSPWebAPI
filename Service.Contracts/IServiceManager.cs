namespace Service.Contracts
{
    public interface IServiceManager
    {
        IProjectService ProjectService { get; }
        IStageService StageService { get; }
        ICardService CardService { get; }
        IAuthenticationService AuthenticationService { get; }
        IUserService UserService { get; }
        IMemberService MemberService { get; }
        IRoleService RoleService { get; }
        ICardMemberService CardMemberService { get; }
    }
}
