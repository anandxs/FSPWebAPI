namespace Entities.ConfigurationModels;

public class SuperAdminConfiguration
{
    public string Section { get; set; } = "SuperAdmin";

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
}
