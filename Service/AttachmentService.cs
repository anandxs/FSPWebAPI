using AutoMapper;
using Azure.Storage.Blobs;
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
    public class AttachmentService : IAttachmentService
    {
        private const string BlobContainerName = "attachments";
        private readonly IRepositoryManager _repositoryManager;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly BlobServiceClient _blobServiceClient;

        public AttachmentService(IRepositoryManager repositoryManager, ILoggerManager logger, IMapper mapper, IHttpContextAccessor contextAccessor, BlobServiceClient blobServiceClient)
        {
            _repositoryManager = repositoryManager;
            _logger = logger;
            _mapper = mapper;
            _contextAccessor = contextAccessor;
            _blobServiceClient = blobServiceClient;
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

        public async Task<(Stream stream, string fileName)> GetAttachmentByIdAsync(Guid projectId, Guid taskId, Guid attachmentId, bool trackChanges)
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

            var containerClient = _blobServiceClient.GetBlobContainerClient(BlobContainerName);
            var blobClient = containerClient.GetBlobClient($"{taskId}/{attachment.FileName}");
            var downloadContent = await blobClient.DownloadContentAsync();

            var stream = new MemoryStream(downloadContent.Value.Content.ToArray());

            return (stream, attachment.FileName);
        }

        public async Task AddAttachmentAsync(Guid projectId, Guid taskId, bool trackChanges)
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

            if (requester.Role.Name != Constants.PROJECT_ROLE_ADMIN && requester.MemberId != task.AssigneeId)
            {
                throw new IncorrectRoleForbiddenRequestException();
            }

            var file = _contextAccessor.HttpContext.Request.Form.Files[0];
            
            using (Stream stream = file.OpenReadStream())
            {
                var containerClient = _blobServiceClient.GetBlobContainerClient(BlobContainerName);
                var blobClient = containerClient.GetBlobClient($"{taskId}/{file.FileName}");
                var exists = await blobClient.ExistsAsync();

                if (exists)
                {
                    throw new DuplicateEntryBadRequest();
                }

                await blobClient.UploadAsync(stream);
            }

            var attachment = new Attachment
            {
                FileName = $"{file.FileName}",
                CreatedAt = DateTime.Now,
            };
            _repositoryManager.AttachmentRepository.AddAttachment(taskId, attachment);
            await _repositoryManager.SaveAsync();
        }

        public async Task DeleteAttachmentAsync(Guid projectId, Guid taskId, Guid attachmentId, bool trackChanges)
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

            if (requester.Role.Name != Constants.PROJECT_ROLE_ADMIN && requester.MemberId != task.AssigneeId)
            {
                throw new IncorrectRoleForbiddenRequestException();
            }

            var attachment = await _repositoryManager.AttachmentRepository.GetAttachmentByIdAsync(taskId, attachmentId, trackChanges);

            if (attachment is null)
            {
                throw new AttachmentNotFoundException(attachmentId);
            }

            var containerClient = _blobServiceClient.GetBlobContainerClient(BlobContainerName);
            var blobClient = containerClient.GetBlobClient($"{taskId}/{attachment.FileName}");
            await blobClient.DeleteAsync();

            _repositoryManager.AttachmentRepository.RemoveAttachment(attachment);
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
