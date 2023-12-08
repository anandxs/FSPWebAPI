using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class ProjectMember
    {
        [Key]
        public Guid Id { get; set; }
        [ForeignKey(nameof(User))]
        public string? MemberId { get; set; }
        public User? User { get; set; }
        [ForeignKey(nameof(Project))]
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }
        [Required(ErrorMessage = "Role is a required field.")]
        [MaxLength(256, ErrorMessage = "Maximum length for role name is 256 characters.")]
        public string? Role { get; set; }
    }
}
