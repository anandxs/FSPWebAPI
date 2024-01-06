namespace Service;

public class ProjectService : IProjectService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly IHttpContextAccessor _contextAccessor;

    public ProjectService(IRepositoryManager repositoryManager, ILoggerManager logger, IMapper mapper, UserManager<User> userManager, IHttpContextAccessor contextAccessor)
    {
        _repositoryManager = repositoryManager;
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
        _contextAccessor = contextAccessor;
    }

    public async Task<IEnumerable<ProjectDto>> GetProjectsUserIsPartOfAsync(bool trackChanges)
    {
        var requesterId = GetRequesterId();
        var entities = await _repositoryManager.ProjectMemberRepository.GetProjectsForMemberAsync(requesterId, trackChanges);
        
        var projects = entities.Select(m => m.Project);
        
        var projectsDto = _mapper.Map<IEnumerable<ProjectDto>>(projects);
        
        return projectsDto;
    }

    public async Task<ProjectDto> GetProjectUserIsPartOfAsync(Guid projectId, bool trackChanges)
    {
        var requesterId = GetRequesterId();
        var entity = await _repositoryManager.ProjectMemberRepository.GetProjectForMemberAsync(requesterId, projectId, trackChanges);
        
        var project = entity?.Project;
        if (entity is null || project is null)
        {
            throw new ProjectNotFoundException(projectId);
        }
        
        var projectDto = _mapper.Map<ProjectDto>(project);

        return projectDto;
    }

    public async Task<ProjectDto> CreateProjectAsync(ProjectForCreationDto project)
    {
        var requesterId = GetRequesterId();
        var requester = await _userManager.FindByIdAsync(requesterId);
        if (requester is null)
        {
            throw new UserNotFoundException(requesterId);
        }

        var projectEntity = _mapper.Map<Project>(project);
        _repositoryManager.ProjectRepository.CreateProject(projectEntity, requesterId);
        
        var member = new ProjectMember
        {
            User = requester,
            Project = projectEntity,
            Role = new Role
            {
                Name = Constants.PROJECT_ROLE_ADMIN,
                Project = projectEntity,
            }
        };
        _repositoryManager.ProjectMemberRepository.AddProjectMember(member);
        await _repositoryManager.SaveAsync();
        
        var projectToReturn = _mapper.Map<ProjectDto>(projectEntity);

        return projectToReturn;
    }

    public async Task UpdateProjectAsync(Guid projectId, ProjectForUpdateDto projectForUpdateDto, bool trackChanges)
    {
        var requesterId = GetRequesterId();
        var entity = await _repositoryManager.ProjectMemberRepository.GetProjectForMemberAsync(requesterId, projectId, trackChanges);

        if (entity == null)
        {
            throw new NotAProjectMemberForbiddenRequestException();
        }
        else if (entity.Role.Name != Constants.PROJECT_ROLE_ADMIN)
        {
            throw new IncorrectRoleForbiddenRequestException();
        }

        _mapper.Map(projectForUpdateDto, entity.Project);
        await _repositoryManager.SaveAsync();
    }

    public async Task ToggleArchive(Guid projectId, bool trackChanges)
    {
        var requesterId = GetRequesterId();
        var entity = await _repositoryManager.ProjectMemberRepository.GetProjectForMemberAsync(requesterId, projectId, trackChanges);

        if (entity == null)
        {
            throw new NotAProjectMemberForbiddenRequestException();
        }
        else if (entity.Role.Name != Constants.PROJECT_ROLE_ADMIN)
        {
            throw new IncorrectRoleForbiddenRequestException();
        }

        var project = entity.Project;

        project.IsActive = !project.IsActive;
        await _repositoryManager.SaveAsync();
    }

    public async Task DeleteProject(Guid projectId, bool trackChanges)
    {
        var requesterId = GetRequesterId();
        var entity = await _repositoryManager.ProjectMemberRepository.GetProjectForMemberAsync(requesterId, projectId, trackChanges);

        if (entity == null)
        {
            throw new NotAProjectMemberForbiddenRequestException();
        }
        else if (entity.Role.Name != Constants.PROJECT_ROLE_ADMIN)
        {
            throw new IncorrectRoleForbiddenRequestException();
        }
        else if (entity.Project.OwnerId != requesterId)
        {
            throw new IncorrectRoleForbiddenRequestException();
        }

        _repositoryManager.ProjectRepository.DeleteProject(entity.Project);
        await _repositoryManager.SaveAsync();
    }

    private string GetRequesterId()
    {
        var claimsIdentity = (ClaimsIdentity)_contextAccessor.HttpContext.User.Identity;
        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

        return claim!.Value;
    }
}
