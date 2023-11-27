namespace Shared.DataTransferObjects
{
    public record ProjectMemberDto
    {
        public Guid Id { get; set; }
        public UserDto User { get; set; }
        public ProjectRoleDto ProjectRole { get; set; }

    }
}
