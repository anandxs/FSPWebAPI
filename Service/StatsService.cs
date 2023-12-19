using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Microsoft.AspNetCore.Http;
using Service.Contracts;
using Shared;
using Shared.DataTransferObjects;
using System.Security.Claims;

namespace Service
{
    public class StatsService : IStatsService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _contextAccessor;

        public StatsService(IRepositoryManager repositoryManager, ILoggerManager logger, IMapper mapper, IHttpContextAccessor contextAccessor)
        {
            _repositoryManager = repositoryManager;
            _logger = logger;
            _mapper = mapper;
            _contextAccessor = contextAccessor;
        }

        public async Task<IEnumerable<TasksPerStageDto>> GetTasksPerStageAsync(Guid projectId, bool trackChanges)
        {
            var requesterId = GetRequesterId();

            var requester = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(projectId, requesterId, trackChanges);

            if (requester is null)
            {
                throw new NotAProjectMemberForbiddenRequestException();
            }
            else if (requester.Role.Name != Constants.PROJECT_ROLE_ADMIN)
            {
                throw new IncorrectRoleForbiddenRequestException();
            }

            var stages = await _repositoryManager.StageRepository.GetAllStagesForProjectAsync(projectId, trackChanges);
            var tasks = await _repositoryManager.TaskRepository.GetAllTasksForProjectAsync(projectId, trackChanges);

            var tasksPerStageDtos = new List<TasksPerStageDto>();

            foreach (var stage in stages)
            {
                tasksPerStageDtos.Add(new TasksPerStageDto
                {
                    Stage = stage.Name,
                    Count = tasks.Where(t => t.StageId.Equals(stage.StageId)).Count(),
                });
            }

            return tasksPerStageDtos;
        }

        private string GetRequesterId()
        {
            var claimsIdentity = (ClaimsIdentity)_contextAccessor.HttpContext.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            return claim!.Value;
        }
    }
}
