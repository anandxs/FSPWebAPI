namespace Entities.Exceptions;

public sealed class ProjectNotFoundException : NotFoundException
{
    public ProjectNotFoundException(Guid id) : base($"Project with id : {id} is not found.")
    {
    }
}
