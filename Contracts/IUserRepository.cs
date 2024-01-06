namespace Contracts;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllUsers(bool trackChanges);
}
