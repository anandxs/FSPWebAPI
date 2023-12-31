﻿using Azure.Storage.Blobs;

namespace Service;

public sealed class ServiceManager : IServiceManager
{
    private readonly Lazy<IProjectService> _projectService;
    private readonly Lazy<IRoleService> _roleService;
    private readonly Lazy<IStageService> _stageService;
    private readonly Lazy<ITaskTypeService> _taskTypeService;
    private readonly Lazy<ITagService> _tagService;
    private readonly Lazy<ITaskService> _taskService;
    private readonly Lazy<ICommentService> _commentService;
    private readonly Lazy<IAttachmentService> _attachmentService;
    private readonly Lazy<IAuthenticationService> _authenticationService;
    private readonly Lazy<IUserService> _userService;
    private readonly Lazy<IMemberService> _memberService;
    private readonly Lazy<IStatsService> _statsService;

    public ServiceManager(
        IRepositoryManager repositoryManager, 
        ILoggerManager logger,
        IMapper mapper,
        UserManager<User> userManager,
        IOptions<JwtConfiguration> jwtConfiguration,
        IOptions<ClientConfiguration> clientConfiguration,
        IOptions<SuperAdminConfiguration> adminConfiguration,
        IEmailService emailService,
        IHttpContextAccessor contextAccessor,
        BlobServiceClient blobServiceClient,
        ITokenManager tokenManager)
    {
        _projectService = new Lazy<IProjectService>(() => new ProjectService(repositoryManager, logger, mapper, userManager, contextAccessor));
        _roleService = new Lazy<IRoleService>(() => new RoleService(repositoryManager, logger, mapper, contextAccessor));
        _stageService = new Lazy<IStageService>(() => new StageService(repositoryManager, logger, mapper, userManager, contextAccessor));
        _taskTypeService = new Lazy<ITaskTypeService>(() => new TaskTypeService(repositoryManager, logger, mapper, contextAccessor));
        _tagService = new Lazy<ITagService>(() => new TagService(repositoryManager, logger, mapper, contextAccessor));
        _taskService = new Lazy<ITaskService>(() => new TaskService(repositoryManager, logger, mapper, contextAccessor));
        _commentService = new Lazy<ICommentService>(() => new CommentService(repositoryManager, logger, mapper, contextAccessor));
        _attachmentService = new Lazy<IAttachmentService>(() => new AttachmentService(repositoryManager, logger, mapper, contextAccessor, blobServiceClient));
        _authenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(repositoryManager, logger, mapper, userManager, emailService, jwtConfiguration, clientConfiguration, adminConfiguration));
        _userService = new Lazy<IUserService>(() => new UserService(repositoryManager, logger, mapper, userManager, emailService, tokenManager));
        _memberService = new Lazy<IMemberService>(() => new MemberService(repositoryManager, logger, mapper, userManager, emailService, clientConfiguration, contextAccessor));
        _statsService = new Lazy<IStatsService>(() => new StatsService(repositoryManager, logger, mapper, contextAccessor));
    }

    public IProjectService ProjectService => _projectService.Value;
    public IRoleService RoleService => _roleService.Value;
    public IStageService StageService => _stageService.Value;
    public ITaskTypeService TaskTypeService => _taskTypeService.Value;
    public ITagService TagService => _tagService.Value;
    public ITaskService TaskService => _taskService.Value;
    public ICommentService CommentService => _commentService.Value;
    public IAttachmentService AttachmentService => _attachmentService.Value;
    public IAuthenticationService AuthenticationService => _authenticationService.Value;
    public IUserService UserService => _userService.Value;
    public IMemberService MemberService => _memberService.Value;
    public IStatsService StatsService => _statsService.Value;

}
