using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface IUserService
    {
        Task<UserDto> GetUser(string userId);
    }
}
