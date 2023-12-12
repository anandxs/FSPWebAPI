namespace Entities.Exceptions
{
    public sealed class CannotModifySuperAdminBadRequestException : ForbiddenRequestException
    {
        public CannotModifySuperAdminBadRequestException() : base($"Cannot modify super admin.")
        {
        }
    }
}
