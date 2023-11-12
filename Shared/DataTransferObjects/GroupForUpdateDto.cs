using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects
{
    public record GroupForUpdateDto
    {
        [Required(ErrorMessage = "Group name is a required field.")]
        public string? Name { get; set; }
    }
}
