using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Identity;

namespace Repository
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILoggerManager _logger;

        public DbInitializer(
            UserManager<User> userManager, 
            RoleManager<IdentityRole> roleManager,
            ILoggerManager logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public void Initialize()
        {
            try
            {
                if (!_roleManager.Roles.Any())
                {
                    _roleManager.CreateAsync(new IdentityRole { Name = "SUPERADMIN" }).GetAwaiter().GetResult();

                    var superAdmin = new User
                    {
                        FirstName = "Super",
                        LastName = "Admin",
                        Email = "admin@mail.com",
                        EmailConfirmed = true,
                        UserName = "admin@mail.com"
                    };

                    _userManager.CreateAsync(superAdmin, "Password1").GetAwaiter().GetResult();

                    _userManager.AddToRoleAsync(superAdmin, "SUPERADMIN").GetAwaiter().GetResult();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Superadmin creation failed.");
                _logger.LogError($"{ex.ToString()}");
            }
        }
    }
}
