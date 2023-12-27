
using Entities.Exceptions;
using Service.Contracts;
using System.Net;

namespace FSPWebAPI.Middlewares
{
    public class TokenValidityCheckerMiddleware : IMiddleware
    {
        private readonly ITokenManager _tokenManager;

        public TokenValidityCheckerMiddleware(ITokenManager tokenManager)
        {
            _tokenManager = tokenManager;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (await _tokenManager.IsCurrentActiveTokenAsync() || !context.User.Identity.IsAuthenticated)
            {
                await next(context);
                return;
            }

            throw new BlockedUserUnauthorizedException();
        }
    }
}
