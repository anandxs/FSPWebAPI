namespace Entities.Exceptions
{
    public sealed class OwnerCannotExitBadRequestException : BadRequestException
    {
        public OwnerCannotExitBadRequestException() : base($"Owner cannot exit project.")
        {
        }
    }
}
