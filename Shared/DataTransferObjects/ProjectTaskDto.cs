namespace Shared.DataTransferObjects
{
    public record ProjectTaskDto(Guid TaskId, string Title, string? Description, bool IsActive, DateTime? DueDate, IncludedStageDto stage);
}
