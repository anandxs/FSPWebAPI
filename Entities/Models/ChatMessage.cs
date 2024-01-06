namespace Entities.Models;

public class ChatMessage
{
    [Column("MessageId")]
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Message is a required field.")]
    [MaxLength(500, ErrorMessage = "Maximum length of message is 500 characters.")]
    public string? Message { get; set; }
    public DateTime SentAt { get; set; }
    [ForeignKey(nameof(Project))]
    public Guid ProjectId { get; set; }
    public Project Project { get; set; }
    [ForeignKey(nameof(User))]
    public string? SenderId { get; set; }
    public User? Sender { get; set; }
}
