using Entities.Models;

namespace Contracts
{
    public interface IAttachmentRepository
    {
        Task<IEnumerable<Attachment>> GetAllTaskAttachmentsAsync(Guid taskId, bool trackChanges);
        Task<Attachment> GetAttachmentByIdAsync(Guid taskId, Guid attachmentId, bool trackChanges);
        void AddAttachment(Guid taskId, Attachment attachment);
        void RemoveAttachment(Attachment attachment);
    }
}
