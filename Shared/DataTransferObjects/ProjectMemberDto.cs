namespace Shared.DataTransferObjects
{
    public record ProjectMemberDto
    {
        public ProjectUserDto User { get; set; }
        public string Role { get; set; }
        public ProjectDto Project { get; set; }
    }
}
