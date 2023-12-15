using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects
{
    public record StageForCreationDto
    {
        [Required(ErrorMessage = "Stage name is a required field.")]
        public string? Name { get; set; }
    }
}
