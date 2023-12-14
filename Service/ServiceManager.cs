using AutoMapper;
using Contracts;
using Entities.ConfigurationModels;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using NETCore.MailKit.Core;
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
        private readonly Lazy<IMemberService> _memberService;
        private readonly Lazy<IRoleService> _roleService;
        private readonly Lazy<ICardMemberService> _cardMemberService;

        public ServiceManager(
            IRepositoryManager repositoryManager, 
            ILoggerManager logger,
            IMapper mapper,
            UserManager<User> userManager,
            IOptions<JwtConfiguration> jwtConfiguration,
            IOptions<ClientConfiguration> clientConfiguration,
            IOptions<SuperAdminConfiguration> adminConfiguration,
            IEmailService emailService,
            IHttpContextAccessor contextAccessor)
        {
            _projectService = new Lazy<IProjectService>(() => new ProjectService(repositoryManager, logger, mapper, userManager));
            _authenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(logger, mapper, userManager, jwtConfiguration, emailService, clientConfiguration, adminConfiguration));
            _groupService = new Lazy<IGroupService>(() => new GroupService(repositoryManager, logger, mapper, userManager));
            _cardService = new Lazy<ICardService>(() => new CardService(repositoryManager, logger, mapper, userManager));
            _userService = new Lazy<IUserService>(() => new UserService(logger, mapper, userManager, emailService, repositoryManager));
            _memberService = new Lazy<IMemberService>(() => new MemberService(repositoryManager, logger, mapper, userManager, emailService));
            _roleService = new Lazy<IRoleService>(() => new RoleService(repositoryManager, logger, mapper));
            _cardMemberService = new Lazy<ICardMemberService>(() => new CardMemberService(repositoryManager, logger, mapper, contextAccessor));
        }

        public IProjectService ProjectService => _projectService.Value;
        public IGroupService GroupService => _groupService.Value;
        public IAuthenticationService AuthenticationService => _authenticationService.Value;
        public ICardService CardService => _cardService.Value;
        public IUserService UserService => _userService.Value;
        public IMemberService MemberService => _memberService.Value;
        public IRoleService RoleService => _roleService.Value;
        public ICardMemberService CardMemberService => _cardMemberService.Value;
    }
}
