namespace Shared.DataTransferObjects
{
    public record TasksPerStageDto
    {
        public string? Stage { get; init; }
        public int Count { get; init; }
    }
}
