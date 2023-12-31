namespace FSPWebAPI.Middlewares
{
    public class UserAccountStatusCheckMiddleware : IMiddleware
    {
        private readonly ITokenManager _tokenManager;

        public UserAccountStatusCheckMiddleware(ITokenManager tokenManager)
        {
            _tokenManager = tokenManager;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (!await _tokenManager.IsCurrentUserBlockedAsync())
            {
                await next(context);
                return;
            }

            throw new BlockedUserUnauthorizedException();
        }
    }
}
