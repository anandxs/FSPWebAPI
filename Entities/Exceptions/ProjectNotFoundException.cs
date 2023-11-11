namespace Entities.Exceptions
{
    public class ProjectNotFoundException : NotFoundException
    {
        public ProjectNotFoundException(Guid id) : base($"Project with id : {id} is not found.")
        {
        }
    }
}
