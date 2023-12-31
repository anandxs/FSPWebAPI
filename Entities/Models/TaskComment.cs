namespace Entities.Models
{
    public class TaskComment
    {
        [Key]
        public Guid CommentId { get; set; }
        [Required(ErrorMessage = "Comment cannot be empty.")]
        [MaxLength(500, ErrorMessage = "Maximum length of comment is 500 characters")]
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        [ForeignKey(nameof(ProjectTask))]
        public Guid TaskId { get; set; }
        public ProjectTask ProjectTask { get; set;}
        [ForeignKey(nameof(User))]
        public string? CommenterId { get; set; }
        public User? Commenter { get; set; }
    }
}
