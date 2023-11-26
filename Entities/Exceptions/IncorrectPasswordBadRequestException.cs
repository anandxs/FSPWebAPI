namespace Entities.Exceptions
{
    public sealed class IncorrectPasswordBadRequestException : BadRequestException
    {
        public IncorrectPasswordBadRequestException() : base("Incorrect password.")
        {
        }
    }
}
