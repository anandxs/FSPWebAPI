namespace Entities.Models;

public class TaskType
{
    [Key]
    [Column("TypeId")]
    public Guid TypeId{ get; set; }
    [Required(ErrorMessage = "Task type is a required field.")]
    [MaxLength(256, ErrorMessage = "Maximum length for task type is 256 characters")]
    public string? Name { get; set; }
    public bool IsActive { get; set; } = true;
    [ForeignKey(nameof(Project))]
    public Guid ProjectId { get; set; }
    public Project Project { get; set; }
}
