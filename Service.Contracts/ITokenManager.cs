namespace Service.Contracts;

public interface ITokenManager
{
    Task<bool> IsCurrentUserBlockedAsync();
    Task BlockUserAsync(string userId);
    Task UnBlockUserAsync(string userId);
    Task<bool> IsCurrentActiveTokenAsync();
    Task DeactivateCurrentAsync();
    Task<bool> IsActiveAsync(string token);
    Task DeactivateAsync(string token);
}
