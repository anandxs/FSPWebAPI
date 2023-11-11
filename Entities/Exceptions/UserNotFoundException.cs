namespace Entities.Exceptions
{
    public class UserNotFoundException : NotFoundException
    {
        public UserNotFoundException(string id) : base($"User with id : {id} is not found.")
        {
        }
    }
}
