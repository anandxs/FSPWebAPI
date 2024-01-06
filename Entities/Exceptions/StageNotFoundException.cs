namespace Entities.Exceptions;

public sealed class StageNotFoundException : NotFoundException
{
    public StageNotFoundException(Guid id) : base($"Stage with id : {id} is not found.")
    {
    }
}
