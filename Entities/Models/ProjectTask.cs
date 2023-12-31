namespace Entities.Models
{
    public class ProjectTask
    {
        [Key]
        public Guid TaskId { get; set; }
        [Required(ErrorMessage = "Title is a required field.")]
        [MaxLength(256, ErrorMessage = "Maximum length of title is 256 characters")]
        public string? Title { get; set; }
        [MaxLength(1000, ErrorMessage = "Maximum length of description is 1000 characters.")]
        public string? Description { get; set; }
        [Range(0, float.MaxValue, ErrorMessage = "Total hours requried cannot be a negative value.")]
        public float TotalHours { get; set; }
        [Range(0, float.MaxValue, ErrorMessage = "Hours spent cannot be a negative value.")]
        public float HoursSpent { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        [ForeignKey(nameof(User))]
        public string? AssigneeId { get; set; }
        public User? Assignee { get; set; }
        [ForeignKey(nameof(Stage))]
        public Guid StageId { get; set; }
        public Stage Stage { get; set; }
        [ForeignKey(nameof(TaskType))]
        public Guid TypeId { get; set; }
        public TaskType Type { get; set; }
        [ForeignKey(nameof(Project))]
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
