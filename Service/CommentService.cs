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
    public class CommentService : ICommentService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _contextAccessor;

        public CommentService(IRepositoryManager repositoryManager, ILoggerManager logger, IMapper mapper, IHttpContextAccessor contextAccessor)
        {
            _repositoryManager = repositoryManager;
            _logger = logger;
            _mapper = mapper;
            _contextAccessor = contextAccessor;
        }

        public async Task<IEnumerable<TaskCommentDto>> GetAllCommentsForTaskAsync(Guid projectId, Guid taskId, bool trackChanges)
        {
            var requesterId = GetRequesterId();

            var requester = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(projectId, requesterId, trackChanges);

            if (requester is null)
            {
                throw new NotAProjectMemberForbiddenRequestException();
            }

            var comments = await _repositoryManager.CommentRepository.GetAllCommentsForTaskAsync(taskId, trackChanges);

            var commentsDto = _mapper.Map<IEnumerable<TaskCommentDto>>(comments);

            return commentsDto;
        }

        public async Task AddCommentToTaskAsync(Guid projectId, Guid taskId, TaskCommentForCreationDto commentDto, bool trackChanges)
        {
            var requesterId = GetRequesterId();

            var requester = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(projectId, requesterId, trackChanges);

            var task = await _repositoryManager.TaskRepository.GetTaskByIdAsync(taskId, trackChanges);

            if (requester is null)
            {
                throw new NotAProjectMemberForbiddenRequestException();
            }

            var comment = _mapper.Map<TaskComment>(commentDto);
            comment.CommenterId = requesterId;
            comment.TaskId = taskId;

            _repositoryManager.CommentRepository.AddCommentToTask(comment);
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
