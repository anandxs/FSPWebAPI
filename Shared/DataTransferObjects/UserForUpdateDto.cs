using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects
{
    public record UserForUpdateDto
    {
        [Required(ErrorMessage = "First name is a required field.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is a required field.")]
        public string LastName { get;set; }
    }
}
