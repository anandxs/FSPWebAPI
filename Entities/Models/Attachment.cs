using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Attachment
    {
        [Key]
        public Guid AttachmentId { get; set; }
        [Required(ErrorMessage = "File name is a required field.")]
        [MaxLength(256, ErrorMessage = "Maximum length of file name is 256 characters.")]
        public string? FileName { get; set; }
        [Required(ErrorMessage = "File URL is a required field.")]
        [MaxLength(2000, ErrorMessage = "Maximum length of file url is 1000 characters.")]
        public string? FileUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        [ForeignKey(nameof(ProjectTask))]
        public Guid TaskId { get; set; }
        public ProjectTask Task { get; set;}
    }
}
