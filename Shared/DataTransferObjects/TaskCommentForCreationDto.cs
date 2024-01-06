using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects;

public record TaskCommentForCreationDto
{
    [Required(ErrorMessage = "Comment cannot be empty.")]
    [MaxLength(500, ErrorMessage = "Maximum length of comment is 500 characters")]
    public string? Comment { get; set; }
}
