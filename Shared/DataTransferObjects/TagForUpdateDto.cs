using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects;

public record TagForUpdateDto
{
    [Required(ErrorMessage = "Tag name is a required field.")]
    [MaxLength(256, ErrorMessage = "Maximum length for tag name is 256 characters")]
    public string? Name { get; set; }
    [Required(ErrorMessage = "Color is a required field.")]
    [MaxLength(7, ErrorMessage = "Maximum length for colour is 7 characters")]
    public string? Colour { get; set; }
}
