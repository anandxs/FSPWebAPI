namespace Entities.Exceptions
{
    public sealed class AttachmentNotFoundException : NotFoundException
    {
        public AttachmentNotFoundException(Guid attachmentId) : base($"Attachment with id : {attachmentId} is not found.")
        {
        }
    }
}
