namespace Entities.Exceptions;

public sealed class UserNotFoundException : NotFoundException
{
    public UserNotFoundException(string id) : base($"User with id : {id} is not found.")
    {
    }
}
