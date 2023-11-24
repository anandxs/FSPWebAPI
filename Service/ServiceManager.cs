﻿using AutoMapper;
using Contracts;
using Entities.ConfigurationModels;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Service.Contracts;

namespace Service
{
    public sealed class ServiceManager : IServiceManager
    {
        private readonly Lazy<IProjectService> _projectService;
        private readonly Lazy<IAuthenticationService> _authenticationService;
        private readonly Lazy<IGroupService> _groupService;
        private readonly Lazy<ICardService> _cardService;
        private readonly Lazy<IUserService> _userService;

        public ServiceManager(
            IRepositoryManager repositoryManager, 
            ILoggerManager logger,
            IMapper mapper,
            UserManager<User> userManager,
            IOptions<JwtConfiguration> configuration)
        {
            _projectService = new Lazy<IProjectService>(() => new ProjectService(repositoryManager, logger, mapper, userManager));
            _authenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(logger, mapper, userManager, configuration));
            _groupService = new Lazy<IGroupService>(() => new GroupService(repositoryManager, logger, mapper, userManager));
            _cardService = new Lazy<ICardService>(() => new CardService(repositoryManager, logger, mapper, userManager));
            _userService = new Lazy<IUserService>(() => new UserService(logger, mapper, userManager));
        }

        public IProjectService ProjectService => _projectService.Value;
        public IGroupService GroupService => _groupService.Value;
        public IAuthenticationService AuthenticationService => _authenticationService.Value;
        public ICardService CardService => _cardService.Value;
        public IUserService UserService => _userService.Value;
    }
}
