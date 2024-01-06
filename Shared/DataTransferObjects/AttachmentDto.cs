namespace Shared.DataTransferObjects;

public record AttachmentDto(Guid AttachmentId, string FileName, DateTime CreatedAt);
