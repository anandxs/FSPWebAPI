using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects;

public record MemberForUpdateDto
{
    [Required(ErrorMessage = "Member id is a required field.")]
    public string MemberId { get; init; }
    [Required(ErrorMessage = "Role is a required field.")]
    public Guid RoleId { get; set; }
}
