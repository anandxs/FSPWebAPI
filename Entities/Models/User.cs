using Microsoft.AspNetCore.Identity;

namespace Entities.Models
{
    public class User : IdentityUser
    {
        [Required(ErrorMessage = "First name is a required field")]
        [MaxLength(256, ErrorMessage = "First name can be of atmost 256 characters")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name is a required field")]
        [MaxLength(256, ErrorMessage = "Last name can be of atmost 256 characters")]
        public string LastName { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public bool IsBlocked { get; set; }
    }
}
