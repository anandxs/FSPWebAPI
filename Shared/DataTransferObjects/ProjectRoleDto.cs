namespace Shared.DataTransferObjects
{
    public record ProjectRoleDto
    {
        public Guid RoleId { get; set; }
        public string Name { get; set; }
    }
}
