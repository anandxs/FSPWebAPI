using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class ProjectRole
    {
        [Key]
        public Guid RoleId { get; set; }
        [Required(ErrorMessage = "Role name is a required field.")]
        [MaxLength(256, ErrorMessage = "Maximum length of role name is 256 characters.")]
        public string? Name { get; set; }
        [ForeignKey(nameof(Project))]
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
