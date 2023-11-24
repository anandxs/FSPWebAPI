using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects
{
    public record UserForPasswordUpdate
    {
        [Required(ErrorMessage = "Current password is required.")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "New password is required.")]
        public string NewPassword { get; set; }
    }
}
