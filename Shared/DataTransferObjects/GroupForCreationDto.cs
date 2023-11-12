using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects
{
    public record GroupForCreationDto
    {
        [Required(ErrorMessage = "Group name is a required field.")]
        public string? Name { get; set; }
    }
}
