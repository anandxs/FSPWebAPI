namespace Entities.Exceptions;

public sealed class DuplicateEntryBadRequest : BadRequestException
{
    public DuplicateEntryBadRequest() : base("An entry with same name already exists.")
    {
    }
}
