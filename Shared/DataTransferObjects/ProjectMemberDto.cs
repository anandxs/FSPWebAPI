namespace Shared.DataTransferObjects
{
    public record ProjectMemberDto
    {
        public UserDto User { get; set; }
        public string Role { get; set; }
    }
}
