using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace Service;

public class TokenManager : ITokenManager
{
    private readonly IMemoryCache _memoryCache;
    private readonly IHttpContextAccessor _contextAccesor;
    private readonly IOptions<JwtConfiguration> _jwtOptions;

    public TokenManager(
            IHttpContextAccessor httpContextAccessor,
            IOptions<JwtConfiguration> jwtOptions,
            IMemoryCache memoryCache)
    {
        _contextAccesor = httpContextAccessor;
        _jwtOptions = jwtOptions;
        _memoryCache = memoryCache;
    }

    public async Task<bool> IsCurrentActiveTokenAsync()
    {
        return await IsActiveAsync(GetCurrentAsync());
    }

    public async Task DeactivateCurrentAsync()
    {
        await DeactivateAsync(GetCurrentAsync());
    }

    public async Task<bool> IsActiveAsync(string token)
    {
        return !_memoryCache.TryGetValue(GetKey(token), out _);
    }

    public async Task DeactivateAsync(string token)
    {
        _memoryCache.Set<string>(GetKey(token), " ", new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(Convert.ToDouble(_jwtOptions.Value.Expires))
        });
    }

    private string GetCurrentAsync()
    {
        var authorizationHeader = _contextAccesor.HttpContext.Request.Headers["Authorization"];

        return authorizationHeader == StringValues.Empty ? string.Empty : authorizationHeader.Single().Split(" ").Last();
    }

    private static string GetKey(string token)
    {
        return $"tokens:{token}:deactivated";
    }

    public async Task<bool> IsCurrentUserBlockedAsync()
    {
        var claimsIdentity = (ClaimsIdentity)_contextAccesor.HttpContext.User.Identity;
        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

        if (claim == null)
        {
            return false;
        }

        var userId = claim.Value;
        var value = _memoryCache.Get<string>(userId);

        if (value == null)
        {
            return false;
        }

        return true;
    }

    public async Task BlockUserAsync(string userId)
    {
        _memoryCache.Set<string>(userId, "blocked", new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(Convert.ToDouble(_jwtOptions.Value.Expires))
        });
    }

    public async Task UnBlockUserAsync(string userId)
    {
        _memoryCache.Remove(userId);
    }
}
