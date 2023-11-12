namespace Entities.Exceptions
{
    public sealed class GroupNotFoundException : NotFoundException
    {
        public GroupNotFoundException(Guid id) : base($"Group with id : {id} is not found.")
        {
        }
    }
}
