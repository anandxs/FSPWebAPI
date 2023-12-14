using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects
{
    public record RoleForUpdateDto
    {
        [Required(ErrorMessage = "Role name is a required field.")]
        [MaxLength(256, ErrorMessage = "Maximum length of role name is 256 characters.")]
        public string? Name { get; init; }
    }
}
