using AutoMapper;
using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Service.Contracts;

namespace Service
{
    public class RoleService : IRoleService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        
        public RoleService(
            IRepositoryManager repositoryManager, 
            ILoggerManager logger, 
            IMapper mapper, 
            UserManager<User> userManager)
        {
            _repositoryManager = repositoryManager;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
        }
    }
}
