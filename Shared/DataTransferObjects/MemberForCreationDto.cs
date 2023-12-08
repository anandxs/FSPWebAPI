using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects
{
    public record MemberForCreationDto
    {
        [Required(ErrorMessage = "Email is a required field.")]
        public string Email { get; init; }
        [Required(ErrorMessage = "Role is a required field.")]
        [MaxLength(256, ErrorMessage = "Maximum length for role name is 256 characters.")]
        public string? Role { get; init; }
    }
}
