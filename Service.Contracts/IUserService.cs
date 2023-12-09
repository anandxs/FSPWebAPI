using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetUsersAsync(bool trackChanges);
        Task<UserDto> GetUserAsync(string userId);
        Task UpdateUserAsync(string userId, UserForUpdateDto userDto);
        Task UpdatePasswordAsync(string userId, UserForPasswordUpdate userDto);
    }
}
