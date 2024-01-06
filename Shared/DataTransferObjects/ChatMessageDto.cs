namespace Shared.DataTransferObjects;

public record ChatMessageDto(Guid Id, string Message, DateTime SentAt, UserDto Sender);
