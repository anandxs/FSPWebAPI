namespace Service.Contracts;

public interface IServiceManager
{
    IProjectService ProjectService { get; }
    IRoleService RoleService { get; }
    IStageService StageService { get; }
    ITaskTypeService TaskTypeService { get; }
    ITagService TagService { get; }
    ITaskService TaskService { get; }
    ICommentService CommentService { get; }
    IAttachmentService AttachmentService { get; }
    IAuthenticationService AuthenticationService { get; }
    IUserService UserService { get; }
    IMemberService MemberService { get; }
    IStatsService StatsService { get; }
}
