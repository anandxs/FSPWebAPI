using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class DefaultProjectRole
    {
        [Key]
        public Guid RoleId { get; set; }
        [Required(ErrorMessage = "Role name is a required field.")]
        [MaxLength(256, ErrorMessage = "Maximum length of role name is 256 characters.")]
        public string? Name { get; set; }
    }
}
