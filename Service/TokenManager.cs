using Entities.ConfigurationModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Service.Contracts;
using System.Security.Claims;

namespace Service
{
    public class TokenManager : ITokenManager
    {
        private readonly IDistributedCache _cache;
        private readonly IHttpContextAccessor _contextAccesor;
        private readonly IOptions<JwtConfiguration> _jwtOptions;

        public TokenManager(IDistributedCache cache,
                IHttpContextAccessor httpContextAccessor
,
                IOptions<JwtConfiguration> jwtOptions)
        {
            _cache = cache;
            _contextAccesor = httpContextAccessor;
            _jwtOptions = jwtOptions;
        }

        public async Task<bool> IsCurrentActiveTokenAsync()
        => await IsActiveAsync(GetCurrentAsync());

        public async Task DeactivateCurrentAsync()
            => await DeactivateAsync(GetCurrentAsync());

        public async Task<bool> IsActiveAsync(string token)
            => await _cache.GetStringAsync(GetKey(token)) == null;

        public async Task DeactivateAsync(string token)
            => await _cache.SetStringAsync(GetKey(token),
                " ", new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow =
                        TimeSpan.FromMinutes(Convert.ToDouble(_jwtOptions.Value.Expires))
                });

        private string GetCurrentAsync()
        {
            var authorizationHeader = _contextAccesor.HttpContext.Request.Headers["Authorization"];

            return authorizationHeader == StringValues.Empty ? string.Empty : authorizationHeader.Single().Split(" ").Last();
        }

        private static string GetKey(string token)
            => $"tokens:{token}:deactivated";

        public async Task<bool> IsCurrentUserBlockedAsync()
        {
            var claimsIdentity = (ClaimsIdentity)_contextAccesor.HttpContext.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim == null)
            {
                return false;
            }

            var userId = claim.Value;
            var value = await _cache.GetStringAsync(userId);

            if (value == null)
            {
                return false;
            }

            return true;
        }

        public async Task BlockUserAsync(string userId)
        {
            await _cache.SetStringAsync(userId,
                "blocked", new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow =
                        TimeSpan.FromMinutes(Convert.ToDouble(_jwtOptions.Value.Expires))
                });
        }

        public async Task UnBlockUserAsync(string userId)
        {
            await _cache.RemoveAsync(userId);
        }
    }
}
