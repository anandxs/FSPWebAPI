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
    public class AttachmentService : IAttachmentService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _contextAccessor;

        public AttachmentService(IRepositoryManager repositoryManager, ILoggerManager logger, IMapper mapper, IHttpContextAccessor contextAccessor)
        {
            _repositoryManager = repositoryManager;
            _logger = logger;
            _mapper = mapper;
            _contextAccessor = contextAccessor;
        }

        public async Task<IEnumerable<AttachmentDto>> GetAllProjectAttachmentsAsync(Guid projectId, Guid taskId, bool trackChanges)
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

            var attachments = await _repositoryManager.AttachmentRepository.GetAllTaskAttachmentsAsync(taskId, trackChanges);

            var attachmentsDto = _mapper.Map<IEnumerable<AttachmentDto>>(attachments);

            return attachmentsDto;
        }

        public async Task<AttachmentDto> GetAttachmentByIdAsync(Guid projectId, Guid taskId, Guid attachmentId, bool trackChanges)
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

            var attachment = await _repositoryManager.AttachmentRepository.GetAttachmentByIdAsync(taskId, attachmentId, trackChanges);

            if (attachment == null)
            {
                throw new AttachmentNotFoundException(attachmentId);
            }

            var attachmentDto = _mapper.Map<AttachmentDto>(attachment);

            return attachmentDto;
        }

        private string GetRequesterId()
        {
            var claimsIdentity = (ClaimsIdentity)_contextAccessor.HttpContext.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            return claim!.Value;
        }
    }
}
