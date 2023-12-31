namespace Entities.Models
{
    public class Role
    {
        [Key]
        public Guid RoleId { get; set; }

        [ForeignKey(nameof(Project))]
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }

        [Required(ErrorMessage = "Role name is a required field.")]
        [MaxLength(256, ErrorMessage = "Maximum length of role name is 256 characters.")]
        public string? Name { get; set; }

        public ICollection<UserInvite> UserInvites { get; set; }
    }
}
