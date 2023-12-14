namespace Shared.DataTransferObjects
{
    public record ProjectMemberDto
    {
        public ProjectUserDto User { get; set; }
        public RoleDto Role { get; set; }
        public ProjectDto Project { get; set; }
    }
}
