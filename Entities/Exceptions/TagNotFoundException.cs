namespace Entities.Exceptions;

public sealed class TagNotFoundException : NotFoundException
{
    public TagNotFoundException(Guid tagId) : base($"Tag with id : {tagId} not found.")
    {
    }
}
