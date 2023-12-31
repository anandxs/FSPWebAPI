namespace Shared.DataTransferObjects
{
    public record ProjectTaskDto(Guid TaskId, string Title, string? Description, bool IsActive, StageDto Stage, TaskTypeDto Type, UserDto Assignee, float TotalHours, float HoursSpent);
}
