namespace Service.Contracts
{
    public interface ITokenManager
    {
        // blacklist user when blocked
        Task<bool> IsCurrentUserBlockedAsync();
        Task BlockUserAsync(string userId);
        Task UnBlockUserAsync(string userId);

        // blacklist access token on logout
        Task<bool> IsCurrentActiveTokenAsync();
        Task DeactivateCurrentAsync();
        Task<bool> IsActiveAsync(string token);
        Task DeactivateAsync(string token);
    }
}
