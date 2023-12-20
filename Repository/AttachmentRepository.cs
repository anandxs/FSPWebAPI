using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class AttachmentRepository : RepositoryBase<Attachment>, IAttachmentRepository
    {
        public AttachmentRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Attachment>> GetAllTaskAttachmentsAsync(Guid taskId, bool trackChanges)
        {
            return await FindByCondition(a => a.TaskId.Equals(taskId), trackChanges)
                        .ToListAsync();
        }

        public async Task<Attachment> GetAttachmentByIdAsync(Guid taskId, Guid attachmentId, bool trackChanges)
        {
            return await FindByCondition(a => a.TaskId.Equals(taskId) && a.AttachmentId.Equals(attachmentId), trackChanges)
                        .SingleOrDefaultAsync();
        }

        public void AddAttachment(Guid taskId, Attachment attachment)
        {
            attachment.TaskId = taskId;
            Create(attachment);
        }

        public void RemoveAttachment(Attachment attachment)
        {
            Delete(attachment);
        }
    }
}
