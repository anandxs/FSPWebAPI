namespace Entities.Exceptions
{
    public sealed class TaskTypeNotFoundException : NotFoundException
    {
        public TaskTypeNotFoundException(Guid id) : base($"Task type of id : {id} is not found.")
        {
        }
    }
}
