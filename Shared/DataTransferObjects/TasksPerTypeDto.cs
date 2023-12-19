namespace Shared.DataTransferObjects
{
    public record TasksPerTypeDto
    {
        public string? Type { get; init; }
        public int Count { get; init; }
    }
}
