using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects
{
    public record ProjectForCreationDto
    {
        [Required(ErrorMessage = "Project name is a required field.")]
        public string? Name { get; set; }
    }
}
