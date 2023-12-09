using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using NETCore.MailKit.Core;
using Service.Contracts;
using Shared;
using Shared.DataTransferObjects;

namespace Service
{
    public class UserService : IUserService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly UserManager<User> _userManager;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public UserService(
            ILoggerManager logger,
            IMapper mapper,
            UserManager<User> userManager,
            IEmailService emailService,
            IRepositoryManager repositoryManager)
        {
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
            _emailService = emailService;
            _repositoryManager = repositoryManager;
        }

        public async Task<IEnumerable<UserDto>> GetUsersAsync(bool trackChanges)
        {
            var users = await _repositoryManager.UserRepository.GetAllUsers(trackChanges);

            var usersDto = _mapper.Map<IEnumerable<UserDto>>(users);

            usersDto = usersDto.Select(userDto =>
            {
                userDto.Role = "USER";
                return userDto;
            });

            return usersDto;
        }

        public async Task<UserDto> GetUserAsync(string userId)
        {
            var user = await GetUserAndCheckIfTheyExistAsync(userId);

            var roles = await _userManager.GetRolesAsync(user);

            var role = roles.Count != 0 ? roles[0] : Constants.GLOBAL_ROLE_USER;

            var userDto = _mapper.Map<UserDto>(user);
            userDto.Role = role;

            return userDto;
        }

        public async Task UpdateUserAsync(string userId, UserForUpdateDto userDto)
        {
            var user = await GetUserAndCheckIfTheyExistAsync(userId);

            _mapper.Map(userDto, user);

            await _userManager.UpdateAsync(user);
        }
        public async Task UpdatePasswordAsync(string userId, UserForPasswordUpdate userDto)
        {
            var user = await GetUserAndCheckIfTheyExistAsync(userId);

            var result = await _userManager.ChangePasswordAsync(user, userDto.CurrentPassword, userDto.NewPassword);

            if (result.Errors.Count() != 0)
            {
                _logger.LogWarn("User gave incorrect password");
                throw new IncorrectPasswordBadRequestException();
            }

            await _emailService.SendAsync(user.Email, "Password Changed", $"<p>Your password has been changed successfully.</p>", true);
        }

        private async Task<User> GetUserAndCheckIfTheyExistAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new UserNotFoundException(userId);
            }

            return user;
        }
    }
}
