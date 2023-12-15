using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects
{
    public record TaskTypeForUpdateDto
    {
        [Required(ErrorMessage = "Task type name is a required field.")]
        public string? Name { get; set; }
    }
}
