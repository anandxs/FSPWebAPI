using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects
{
    public record TaskForUpdateDto
    {
        [Required(ErrorMessage = "Title is a required field.")]
        [MaxLength(256, ErrorMessage = "Maximum length of title is 256 characters")]
        public string Title { get; set; }
        [MaxLength(1000, ErrorMessage = "Maximum length of description is 1000 characters.")]
        public string? Description { get; set; }
        [Range(0, float.MaxValue, ErrorMessage = "Total hours requried cannot be a negative value.")]
        public float TotalHours { get; set; }
        [Range(0, float.MaxValue, ErrorMessage = "Hours spent cannot be a negative value.")]
        public float HoursSpent { get; set; }
        public string? AssigneeId { get; set; }
        [Required(ErrorMessage = "Stage id is a required field.")]
        public Guid StageId { get; set; }
        [Required(ErrorMessage = "Type id is a required field.")]
        public Guid TypeId { get; set; }
    }
}
