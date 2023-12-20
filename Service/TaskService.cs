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
    public class TaskService : ITaskService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _contextAccessor;

        public TaskService(IRepositoryManager repositoryManager, ILoggerManager logger, IMapper mapper, IHttpContextAccessor contextAccessor)
        {
            _repositoryManager = repositoryManager;
            _logger = logger;
            _mapper = mapper;
            _contextAccessor = contextAccessor;
        }

        public async Task<IEnumerable<ProjectTaskDto>> GetAllTasksForProjectAsync(Guid projectId, bool trackChanges)
        {
            var requesterId = GetRequesterId();

            var requester = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(projectId, requesterId, false);

            if (requester is null)
            {
                throw new NotAProjectMemberForbiddenRequestException();
            }

            var tasks = await _repositoryManager.TaskRepository.GetAllTasksForProjectAsync(projectId, trackChanges);

            var tasksDto = _mapper.Map<IEnumerable<ProjectTaskDto>>(tasks);

            return tasksDto;
        }

        public async Task<ProjectTaskDto> GetTaskByIdAsync(Guid projectId, Guid taskId, bool trackChanges)
        {
            var requesterId = GetRequesterId();

            var requester = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(projectId, requesterId, trackChanges);

            if (requester is null)
            {
                throw new NotAProjectMemberForbiddenRequestException();
            }

            var task = await _repositoryManager.TaskRepository.GetTaskByIdAsync(taskId, trackChanges);

            if (task == null)
            {
                throw new TaskNotFoundException(taskId);
            }

            var taskDto = _mapper.Map<ProjectTaskDto>(task);

            return taskDto;
        }

        public async Task<ProjectTaskDto> CreateTaskAsync(Guid projectId, TaskForCreationDto taskForCreationDto, bool trackChanges)
        {
            var requesterId = GetRequesterId();

            var requester = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(projectId, requesterId, trackChanges);

            if (requester is null)
            {
                throw new NotAProjectMemberForbiddenRequestException();
            }
            else if (requester.Role.Name != Constants.PROJECT_ROLE_ADMIN)
            {
                throw new IncorrectPasswordBadRequestException();
            }

            var task = _mapper.Map<ProjectTask>(taskForCreationDto);

            if (task.AssigneeId != null)
            {
                var member = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(projectId, task.AssigneeId, trackChanges);

                if (member == null)
                {
                    throw new NotAProjectMemberBadRequestException(task.AssigneeId);
                }
            }

            var stage = await _repositoryManager.StageRepository.GetStageByIdAsync(projectId, task.StageId, trackChanges);

            if (stage == null)
            {
                throw new StageNotFoundException(task.StageId);
            }

            var type = await _repositoryManager.TaskTypeRepository.GetTaskTypeByIdAsync(projectId, task.TypeId, trackChanges);

            if (type == null)
            {
                throw new TaskTypeNotFoundException(task.TypeId);
            }

            task.ProjectId = projectId;

            _repositoryManager.TaskRepository.CreateTask(task);
            await _repositoryManager.SaveAsync();

            var taskDto = _mapper.Map<ProjectTaskDto>(task);

            return taskDto;
        }

        public async Task UpdateTaskAsync(Guid projectId, Guid taskId, TaskForUpdateDto taskForUpdateDto, bool trackChanges)
        {
            var requesterId = GetRequesterId();

            var requester = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(projectId, requesterId, trackChanges);

            if (requester is null)
            {
                throw new NotAProjectMemberForbiddenRequestException();
            }

            if (requester.Role.Name != Constants.GLOBAL_ROLE_SUPERADMIN && taskForUpdateDto.AssigneeId != null)
            {
                throw new IncorrectRoleForbiddenRequestException();
            }

            var task = await _repositoryManager.TaskRepository.GetTaskByIdAsync(taskId, trackChanges);

            if (task == null)
            {
                throw new TaskNotFoundException(taskId);
            }

            if (requester.Role.Name != Constants.PROJECT_ROLE_ADMIN && requester.User.Id != task.AssigneeId)
            {
                throw new IncorrectRoleForbiddenRequestException();
            }

            _mapper.Map(taskForUpdateDto, task);
            await _repositoryManager.SaveAsync();
        }

        public async Task DeleteTaskAsync(Guid projectId, Guid taskId, bool trackChanges)
        {
            var requesterId = GetRequesterId();

            var requester = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(projectId, requesterId, trackChanges);

            if (requester is null)
            {
                throw new NotAProjectMemberForbiddenRequestException();
            }
            else if (requester.Role.Name != Constants.PROJECT_ROLE_ADMIN)
            {
                throw new IncorrectPasswordBadRequestException();
            }

            var task = await _repositoryManager.TaskRepository.GetTaskByIdAsync(taskId, trackChanges);

            if (task == null)
            {
                throw new TaskNotFoundException(taskId);
            }

            _repositoryManager.TaskRepository.DeleteTask(task);
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
