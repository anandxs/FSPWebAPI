namespace Entities.Exceptions
{
    public sealed class TaskNotFoundException : NotFoundException
    {
        public TaskNotFoundException(Guid id) : base($"Task with id : {id} not found.")
        {
        }
    }
}
