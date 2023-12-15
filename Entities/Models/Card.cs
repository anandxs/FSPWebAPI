using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Card
    {
        [Key]
        public Guid CardId { get; set; }
        [Required(ErrorMessage = "Card title is a required field.")]
        [MaxLength(256, ErrorMessage = "Maximum length of title is 256 characters")]
        public string? Title { get; set; }
        [MaxLength(1000, ErrorMessage = "Maximum length of description is 1000 characters.")]
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime? DueDate { get; set; }
        public DateTime CreatedAt { get; set; }
        [ForeignKey(nameof(User))]
        public string? CreatorId { get; set; }
        public User? Creator{ get; set; }
        [ForeignKey(nameof(Group))]
        public Guid GroupId { get; set; }
        public Stage Group { get; set; }
    }
}
