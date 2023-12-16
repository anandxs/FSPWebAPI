namespace Shared.DataTransferObjects
{
    public record TaskCommentDto(Guid CommentId, string Comment, DateTime CreatedAt, UserDto Commenter);
}
