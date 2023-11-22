using FSPWebAPI.Presentation.ActionFilters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace FSPWebAPI.Presentation.Controllers
{
    [Route("api/token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IServiceManager _service;
        public TokenController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet("refresh")]
        public async Task<IActionResult> Refresh()
        {
            if (!(Request.Cookies.TryGetValue("X-Access-Token", out var accessToken) && Request.Cookies.TryGetValue("X-Refresh-Token", out var refreshToken)))
            {
                return BadRequest();
            }

            var tokenDtoToReturn = await _service.AuthenticationService.RefreshToken(new TokenDto(accessToken, refreshToken));
            
            var options = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                MaxAge = TimeSpan.FromDays(30),
                Secure = true
            };

            HttpContext.Response.Cookies.Append("X-Access-Token", tokenDtoToReturn.AccessToken, options);
            HttpContext.Response.Cookies.Append("X-Refresh-Token", tokenDtoToReturn.RefreshToken, options);

            return Ok(tokenDtoToReturn);
        }
    }
}
