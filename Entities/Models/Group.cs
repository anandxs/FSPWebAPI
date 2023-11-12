using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Group
    {
        [Column("GroupId")]
        public Guid GroupId { get; set; }
        [Required(ErrorMessage = "Group name is required field.")]
        [MaxLength(256, ErrorMessage = "Maximum length for group name is 256 characters")]
        public string? Name { get; set; }
        public bool IsActive { get; set; } = true;
        [ForeignKey(nameof(Project))]
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
