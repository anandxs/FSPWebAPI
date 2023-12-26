using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface IAttachmentService
    {
        Task<IEnumerable<AttachmentDto>> GetAllProjectAttachmentsAsync(Guid projectId, Guid taskId, bool trackChanges);
        Task<(Stream stream, string fileName)> GetAttachmentByIdAsync(Guid projectId, Guid taskId, Guid attachmentId, bool trackChanges);
        Task AddAttachmentAsync(Guid projectId, Guid taskId, bool trackChanges);
        Task DeleteAttachmentAsync(Guid projectId, Guid taskId, Guid attachmentId, bool trackChanges);
    }
}
