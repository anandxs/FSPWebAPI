﻿using Entities;

namespace Service;

public class StageService : IStageService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly IHttpContextAccessor _contextAccessor;

    public StageService(IRepositoryManager repositoryManager, ILoggerManager logger, IMapper mapper, UserManager<User> userManager, IHttpContextAccessor contextAccessor)
    {
        _repositoryManager = repositoryManager;
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
        _contextAccessor = contextAccessor;
    }

    public async Task<IEnumerable<StageDto>> GetAllStagesForProjectAsync(Guid projectId, bool trackChanges)
    {
        var requesterId = GetRequesterId();
        var requester = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(projectId, requesterId, false);

        if (requester is null)
        {
            throw new NotAProjectMemberForbiddenRequestException();
        }

        var stages = await _repositoryManager.StageRepository.GetAllStagesForProjectAsync(projectId, trackChanges);
        var stagesDto = _mapper.Map<IEnumerable<StageDto>>(stages);

        return stagesDto;
    }

    public async Task<StageDto> GetStageByIdAsync(Guid projectId, Guid stageId, bool trackChanges)
    {
        var requesterId = GetRequesterId();
        var requester = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(projectId, requesterId, false);

        if (requester is null)
        {
            throw new NotAProjectMemberForbiddenRequestException();
        }

        var stage = await _repositoryManager.StageRepository.GetStageByIdAsync(projectId, stageId, trackChanges);

        if (stage is null)
        {
            throw new StageNotFoundException(stageId);
        }

        var stageDto = _mapper.Map<StageDto>(stage);

        return stageDto;
    }

    public async Task<StageDto> CreateStageAsync(Guid projectId, StageForCreationDto stageForCreation, bool trackChanges)
    {
        var requesterId = GetRequesterId();
        var requester = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(projectId, requesterId, false);

        if (requester is null)
        {
            throw new NotAProjectMemberForbiddenRequestException();
        }
        else if (requester.Role.Name != Constants.PROJECT_ROLE_ADMIN)
        {
            throw new IncorrectRoleForbiddenRequestException();
        }

        var project = await _repositoryManager.ProjectRepository.GetProjectByIdAsync(projectId, trackChanges);

        if (project is null)
        {
            throw new ProjectNotFoundException(projectId);
        }

        var existingStage = await _repositoryManager.StageRepository.GetStageByNameAsync(projectId, stageForCreation.Name, false);

        if (existingStage != null)
        {
            throw new DuplicateEntryBadRequest();
        }

        var stage = _mapper.Map<Stage>(stageForCreation);

        _repositoryManager.StageRepository.CreateStage(stage, projectId);
        await _repositoryManager.SaveAsync();

        var stageDto = _mapper.Map<StageDto>(stage);

        return stageDto;
    }
    public async Task UpdateStageAsync(Guid projectId, Guid stageId, StageForUpdateDto stageForUpdateDto, bool trackChanges)
    {
        var requesterId = GetRequesterId();
        var requester = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(projectId, requesterId, false);

        if (requester is null)
        {
            throw new NotAProjectMemberForbiddenRequestException();
        }
        else if (requester.Role.Name != Constants.PROJECT_ROLE_ADMIN)
        {
            throw new IncorrectRoleForbiddenRequestException();
        }

        var existingStage = await _repositoryManager.StageRepository.GetStageByNameAsync(projectId, stageForUpdateDto.Name, false);

        if (existingStage != null)
        {
            throw new DuplicateEntryBadRequest();
        }

        var stage = await _repositoryManager.StageRepository.GetStageByIdAsync(projectId, stageId, trackChanges);

        if (stage == null)
        {
            throw new StageNotFoundException(stageId);
        }

        _mapper.Map(stageForUpdateDto, stage);
        await _repositoryManager.SaveAsync();
    }

    public async Task ToggleStageArchiveStatusAsync(Guid projectId, Guid stageId, bool trackChanges)
    {
        var requesterId = GetRequesterId();
        var requester = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(projectId, requesterId, false);

        if (requester is null)
        {
            throw new NotAProjectMemberForbiddenRequestException();
        }
        else if (requester.Role.Name != Constants.PROJECT_ROLE_ADMIN)
        {
            throw new IncorrectRoleForbiddenRequestException();
        }

        var stage = await _repositoryManager.StageRepository.GetStageByIdAsync(projectId, stageId, trackChanges);

        if (stage == null)
        {
            throw new StageNotFoundException(stageId);
        }

        stage.IsActive = !stage.IsActive; // maybe move to repository layer
        await _repositoryManager.SaveAsync();
    }

    public async Task DeleteStageAsync(Guid projectId, Guid stageId, bool trackChanges)
    {
        var requesterId = GetRequesterId();
        var requester = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(projectId, requesterId, false);

        if (requester is null)
        {
            throw new NotAProjectMemberForbiddenRequestException();
        }
        else if (requester.Role.Name != Constants.PROJECT_ROLE_ADMIN)
        {
            throw new IncorrectRoleForbiddenRequestException();
        }

        var stage = await _repositoryManager.StageRepository.GetStageByIdAsync(projectId, stageId, trackChanges);

        if (stage == null)
        {
            throw new StageNotFoundException(stageId);
        }

        try
        {
            _repositoryManager.StageRepository.DeleteStage(stage);
            await _repositoryManager.SaveAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError("");
            _logger.LogError(ex.ToString());
            _logger.LogError("");
            throw new StageNotEmptyBadRequestException();
        }
    }

    private string GetRequesterId()
    {
        var claimsIdentity = (ClaimsIdentity)_contextAccessor.HttpContext.User.Identity;
        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

        return claim!.Value;
    }
}
