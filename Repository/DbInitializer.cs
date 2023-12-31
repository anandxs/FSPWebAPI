namespace Repository
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILoggerManager _logger;
        private readonly IOptions<SuperAdminConfiguration> _options;
        private readonly SuperAdminConfiguration _superAdminConfiguration;

        public DbInitializer(
            UserManager<User> userManager, 
            RoleManager<IdentityRole> roleManager,
            ILoggerManager logger,
            IOptions<SuperAdminConfiguration> options)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
            _options = options;
            _superAdminConfiguration = _options.Value;
        }

        public void Initialize()
        {
            try
            {
                if (!_roleManager.Roles.Any())
                {
                    _roleManager.CreateAsync(new IdentityRole { Name = Constants.GLOBAL_ROLE_SUPERADMIN }).GetAwaiter().GetResult();

                    var superAdmin = new User
                    {
                        FirstName = _superAdminConfiguration.FirstName,
                        LastName = _superAdminConfiguration.LastName,
                        Email = _superAdminConfiguration.Email,
                        EmailConfirmed = true,
                        UserName = _superAdminConfiguration.Email
                    };

                    var password = Environment.GetEnvironmentVariable("SADMINPWD");

                    _userManager.CreateAsync(superAdmin, password).GetAwaiter().GetResult();

                    _userManager.AddToRoleAsync(superAdmin, Constants.GLOBAL_ROLE_SUPERADMIN).GetAwaiter().GetResult();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not create super admin");
                _logger.LogError(ex.Message);
            }
        }
    }
}
