namespace Entities.Models
{
    public class UserInvite
    {
        public int Id { get; set; }
        [ForeignKey(nameof(Project))]
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }
        [Required(ErrorMessage = "Email is a required field.")]
        [MaxLength(256, ErrorMessage = "Maximum number of characters for email is 256.")]
        public string? Email { get; set; }
        [ForeignKey(nameof(Role))]
        public Guid RoleId { get; set; }
        public Role Role { get; set; }
        [Required(ErrorMessage = "Status is a required field.")]
        [MaxLength(10, ErrorMessage = "Maximum number of characters for invite status is 10.")]
        public string? Status { get; set; }
    }
}
