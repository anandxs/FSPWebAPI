using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Project
    {
        [Column("ProjectId")]
        public Guid ProjectId { get; set; }
        [Required(ErrorMessage = "Project name is a required field.")]
        [MaxLength(256, ErrorMessage = "Maximum length for project name is 256 characters.")]
        public string? Name { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        [ForeignKey(nameof(User))]
        public string OwnerId { get; set; }
        public User User { get; set; }

        public ICollection<ProjectTask> Tasks { get; set; }
    }
}
