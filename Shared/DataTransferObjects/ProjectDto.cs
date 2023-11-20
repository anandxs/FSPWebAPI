namespace Shared.DataTransferObjects
{
    public record ProjectDto(Guid ProjectId, string Name, DateTime CreatedAt, bool IsActive, string OwnerId);
}
