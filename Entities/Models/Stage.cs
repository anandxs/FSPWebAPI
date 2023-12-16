using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Stage
    {
        [Column("StageId")]
        public Guid StageId { get; set; }
        [Required(ErrorMessage = "Stage name is required field.")]
        [MaxLength(256, ErrorMessage = "Maximum length for stage name is 256 characters")]
        public string? Name { get; set; }
        public bool IsActive { get; set; } = true;
        [ForeignKey(nameof(Project))]
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }

        public ICollection<ProjectTask> Tasks { get; set; }
    }
}
