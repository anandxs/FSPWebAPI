using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects;

public record ResetPasswordDto
{
    [Required]
    public string UserId{ get; set; }

    [Required]
    public string Code { get; set; }

    [Required]
    public string NewPassword { get; set; }
}
