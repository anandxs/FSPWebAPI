﻿using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Service.Contracts;
using Shared;
using Shared.DataTransferObjects;

namespace Service
{
    public class StageService : IStageService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public StageService(IRepositoryManager repositoryManager, ILoggerManager logger, IMapper mapper, UserManager<User> userManager)
        {
            _repositoryManager = repositoryManager;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<IEnumerable<StageDto>> GetAllStagesForProjectAsync(Guid projectId, string requesterId, bool trackChanges)
        {
            var requester = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(projectId, requesterId, false);

            if (requester is null)
            {
                throw new NotAProjectMemberForbiddenRequestException();
            }

            var stages = await _repositoryManager.StageRepository.GetAllStagesForProjectAsync(projectId, trackChanges);
            var stagesDto = _mapper.Map<IEnumerable<StageDto>>(stages);

            return stagesDto;
        }

        public async Task<StageDto> GetStageByIdAsync(Guid stageId, string requesterId, bool trackChanges)
        {
            var stage = await _repositoryManager.StageRepository.GetStageByIdAsync(stageId, trackChanges);

            if (stage is null)
            {
                throw new StageNotFoundException(stageId);
            }

            var requester = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(stage.ProjectId, requesterId, false);

            if (requester is null)
            {
                throw new NotAProjectMemberForbiddenRequestException();
            }

            var stageDto = _mapper.Map<StageDto>(stage);

            return stageDto;
        }

        public async Task<StageDto> CreateStageAsync(Guid projectId, string requesterId, StageForCreationDto stageForCreation, bool trackChanges)
        {
            var requester = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(projectId, requesterId, false);

            if (requester is null)
            {
                throw new NotAProjectMemberForbiddenRequestException();
            }
            else if (requester.Role.Name != Constants.PROJECT_ROLE_ADMIN)
            {
                throw new IncorrectRoleForbiddenRequestException();
            }

            var project = requester.Project;

            if (project is null)
            {
                throw new ProjectNotFoundException(projectId);
            }

            var stage = _mapper.Map<Stage>(stageForCreation);

            _repositoryManager.StageRepository.CreateStage(stage, projectId);
            await _repositoryManager.SaveAsync();

            var stageDto = _mapper.Map<StageDto>(stage);

            return stageDto;
        }
        public async Task UpdateStageAsync(Guid stageId, string requesterId, StageForUpdateDto stageForUpdateDto, bool trackChanges)
        {
            var stage = await _repositoryManager.StageRepository.GetStageByIdAsync(stageId, trackChanges);

            if (stage == null)
            {
                throw new StageNotFoundException(stageId);
            }

            var requester = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(stage.ProjectId, requesterId, false);

            if (requester is null)
            {
                throw new NotAProjectMemberForbiddenRequestException();
            }
            else if (requester.Role.Name != Constants.PROJECT_ROLE_ADMIN)
            {
                throw new IncorrectRoleForbiddenRequestException();
            }

            _mapper.Map(stageForUpdateDto, stage);
            await _repositoryManager.SaveAsync();
        }

        public async Task ToggleStageArchiveStatusAsync(Guid stageId, string requesterId, bool trackChanges)
        {
            var stage = await _repositoryManager.StageRepository.GetStageByIdAsync(stageId, trackChanges);

            if (stage == null)
            {
                throw new StageNotFoundException(stageId);
            }

            var requester = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(stage.ProjectId, requesterId, false);

            if (requester is null)
            {
                throw new NotAProjectMemberForbiddenRequestException();
            }
            else if (requester.Role.Name != Constants.PROJECT_ROLE_ADMIN)
            {
                throw new IncorrectRoleForbiddenRequestException();
            }

            stage.IsActive = !stage.IsActive; // maybe move to repository layer
            await _repositoryManager.SaveAsync();
        }

        public async Task DeleteStageAsync(Guid stageId, string requesterId, bool trackChanges)
        {
            var stage = await _repositoryManager.StageRepository.GetStageByIdAsync(stageId, trackChanges);

            if (stage == null)
            {
                throw new StageNotFoundException(stageId);
            }

            var requester = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(stage.ProjectId, requesterId, false);

            if (requester is null)
            {
                throw new NotAProjectMemberForbiddenRequestException();
            }
            else if (requester.Role.Name != Constants.PROJECT_ROLE_ADMIN)
            {
                throw new IncorrectRoleForbiddenRequestException();
            }

            _repositoryManager.StageRepository.DeleteStage(stage);
            await _repositoryManager.SaveAsync();
        }
    }
}