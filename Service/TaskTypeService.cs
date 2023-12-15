using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Service.Contracts;
using Shared;
using Shared.DataTransferObjects;
using System.Security.Claims;

namespace Service
{
    public class TaskTypeService : ITaskTypeService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _contextAccessor;

        public TaskTypeService(IRepositoryManager repositoryManager, ILoggerManager loggerManager, IMapper mapper, IHttpContextAccessor contextAccessor)
        {
            _repositoryManager = repositoryManager;
            _logger = loggerManager;
            _mapper = mapper;
            _contextAccessor = contextAccessor;
        }

        public async Task<IEnumerable<TaskTypeDto>> GetAllTaskTypesForProjectAsync(Guid projectId, bool trackChanges)
        {
            var requesterId = GetRequesterId();

            var requester = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(projectId, requesterId, false);

            if (requester is null)
            {
                throw new NotAProjectMemberForbiddenRequestException();
            }

            var types = await _repositoryManager.TaskTypeRepository.GetAllTaskTypesForProjectAsync(projectId, trackChanges);

            var typesDto = _mapper.Map<IEnumerable<TaskTypeDto>>(types);

            return typesDto;
        }

        public async Task<TaskTypeDto> GetTaskTypeByIdAsync(Guid typeId, bool trackChanges)
        {
            var requesterId = GetRequesterId();

            var type = await _repositoryManager.TaskTypeRepository.GetTaskTypeByIdAsync(typeId, trackChanges);

            if (type == null)
            {
                throw new TaskTypeNotFoundException(typeId);
            }

            var requester = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(type.ProjectId, requesterId, false);

            if (requester is null)
            {
                throw new NotAProjectMemberForbiddenRequestException();
            }

            var typeDto = _mapper.Map<TaskTypeDto>(type);

            return typeDto;
        }

        public async Task<TaskTypeDto> CreateTaskTypeAsync(Guid projectId, TaskTypeForCreationDto taskTypeForCreation, bool trackChanges)
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

            var project = requester.Project;

            if (project is null)
            {
                throw new ProjectNotFoundException(projectId);
            }

            var type = _mapper.Map<TaskType>(taskTypeForCreation);

            _repositoryManager.TaskTypeRepository.CreateTaskType(type, projectId);
            await _repositoryManager.SaveAsync();

            var typeDto = _mapper.Map<TaskTypeDto>(type);

            return typeDto;
        }

        public async Task UpdateTaskTypeAsync(Guid typeId, TaskTypeForUpdateDto taskTypeForUpdateDto, bool trackChanges)
        {
            var requesterId = GetRequesterId();

            var type = await _repositoryManager.TaskTypeRepository.GetTaskTypeByIdAsync(typeId, trackChanges);

            if (type == null)
            {
                throw new TaskTypeNotFoundException(typeId);
            }

            var requester = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(type.ProjectId, requesterId, false);

            if (requester is null)
            {
                throw new NotAProjectMemberForbiddenRequestException();
            }
            else if (requester.Role.Name != Constants.PROJECT_ROLE_ADMIN)
            {
                throw new IncorrectRoleForbiddenRequestException();
            }

            _mapper.Map(taskTypeForUpdateDto, type);
            await _repositoryManager.SaveAsync();
        }

        public async Task ToggleTaskTypeArchiveStatusAsync(Guid typeId, bool trackChanges)
        {
            var requesterId = GetRequesterId();

            var type = await _repositoryManager.TaskTypeRepository.GetTaskTypeByIdAsync(typeId, trackChanges);

            if (type == null)
            {
                throw new TaskTypeNotFoundException(typeId);
            }

            var requester = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(type.ProjectId, requesterId, false);

            if (requester is null)
            {
                throw new NotAProjectMemberForbiddenRequestException();
            }
            else if (requester.Role.Name != Constants.PROJECT_ROLE_ADMIN)
            {
                throw new IncorrectRoleForbiddenRequestException();
            }

            type.IsActive = !type.IsActive;
            await _repositoryManager.SaveAsync();
        }

        public async Task DeleteTaskTypeAsync(Guid typeId, bool trackChanges)
        {
            var requesterId = GetRequesterId();

            var type = await _repositoryManager.TaskTypeRepository.GetTaskTypeByIdAsync(typeId, trackChanges);

            if (type == null)
            {
                throw new TaskTypeNotFoundException(typeId);
            }

            var requester = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(type.ProjectId, requesterId, false);

            if (requester is null)
            {
                throw new NotAProjectMemberForbiddenRequestException();
            }
            else if (requester.Role.Name != Constants.PROJECT_ROLE_ADMIN)
            {
                throw new IncorrectRoleForbiddenRequestException();
            }

            _repositoryManager.TaskTypeRepository.DeleteTaskType(type);
            await _repositoryManager.SaveAsync();
        }

        private string GetRequesterId()
        {
            var claimsIdentity = (ClaimsIdentity)_contextAccessor.HttpContext.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            return claim!.Value;
        }
    }
}
