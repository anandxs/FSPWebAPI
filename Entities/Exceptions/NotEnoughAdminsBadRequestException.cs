namespace Entities.Exceptions
{
    public sealed class NotEnoughAdminsBadRequestException : BadRequestException
    {
        public NotEnoughAdminsBadRequestException() : base($"Project needs to have atleast one admin.")
        {
        }
    }
}
