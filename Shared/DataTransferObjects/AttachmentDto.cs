namespace Shared.DataTransferObjects
{
    public record AttachmentDto(Guid AttachmentId, string FileName, string FileUrl, DateTime CreatedAt);
}
