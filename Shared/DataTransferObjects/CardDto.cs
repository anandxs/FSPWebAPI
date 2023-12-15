namespace Shared.DataTransferObjects
{
    public record CardDto(Guid CardId, string Title, string? Description, bool IsActive, DateTime? DueDate, IncludedStageDto stage);
}
