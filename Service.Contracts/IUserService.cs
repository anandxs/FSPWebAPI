using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface IUserService
    {
        Task<UserDto> GetUserAsync(string userId);
        Task UpdateUserAsync(string userId, UserForUpdateDto userDto);
        Task UpdatePasswordAsync(string userId, UserForPasswordUpdate userDto);
    }
}
