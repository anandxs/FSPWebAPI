using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface IAttachmentService
    {
        Task<IEnumerable<AttachmentDto>> GetAllProjectAttachmentsAsync(Guid projectId, Guid taskId, bool trackChanges);
        Task<AttachmentDto> GetAttachmentByIdAsync(Guid projectId, Guid taskId, Guid attachmentId, bool trackChanges);
        //Task<AttachmentDto> AddAttachmentAsync(Guid projectId, Guid taskId, string attachmentForCreation, bool trackChanges);
        //void RemoveAttachmentAsync(Guid projectId, Guid taskId, Guid attachmentId, bool trackChanges);
    }
}
