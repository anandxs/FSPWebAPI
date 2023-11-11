using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects
{
    public record ProjectForUpdateDto
    {
        [Required(ErrorMessage = "Project name is a required field.")]
        public string? Name { get; set; }
    }
}
