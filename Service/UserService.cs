﻿using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public UserService(ILoggerManager logger, IMapper mapper, UserManager<User> userManager)
        {
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<UserDto> GetUser(string userId)
        {
            var user = await GetUserAndCheckIfTheyExistAsync(userId);

            var userDto = _mapper.Map<UserDto>(user);

            return userDto;
        }

        public async Task UpdateUser(string userId, UserForUpdateDto userDto)
        {
            var user = await GetUserAndCheckIfTheyExistAsync(userId);

            _mapper.Map(userDto, user);

            await _userManager.UpdateAsync(user);
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