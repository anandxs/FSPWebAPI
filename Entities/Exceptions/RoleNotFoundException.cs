namespace Entities.Exceptions
{
    public sealed class RoleNotFoundException : NotFoundException
    {
        public RoleNotFoundException(Guid id) : base($"Role with id : {id} not foound.")
        {
        }
    }
}
