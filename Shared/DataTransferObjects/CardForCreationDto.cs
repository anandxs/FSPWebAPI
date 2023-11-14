using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects
{
    public record CardForCreationDto
    {
        [Required(ErrorMessage = "Card title is a required field.")]
        [MaxLength(256, ErrorMessage = "Maximum length of title is 256 characters")]
        public string Title { get; set; }
        [MaxLength(1000, ErrorMessage = "Maximum length of description is 1000 characters.")]
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
