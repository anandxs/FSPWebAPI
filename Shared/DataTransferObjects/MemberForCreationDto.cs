using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects
{
    public record MemberForCreationDto
    {
        [Required(ErrorMessage = "Email is a required field.")]
        public string Email { get; init; }
        [Required(ErrorMessage = "RoleId is a required field.")]
        public Guid RoleId { get; init; }
    }
}
