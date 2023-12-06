using FSPWebAPI.Presentation.ActionFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace FSPWebAPI.Presentation.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IServiceManager _service;

        public AuthenticationController(IServiceManager service)
        {
            _service = service;
        }

        /// <summary>
        /// Registers user with given email, password, first name and last name
        /// </summary>
        /// <param name="userForRegistration"></param>
        /// <returns>Created at status code</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
        {
            var result = await _service.AuthenticationService.RegisterUser(userForRegistration);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }

                return BadRequest(ModelState);
            }

            return StatusCode(201);
        }

        [HttpPost("login")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto user)
        {
            if (!await _service.AuthenticationService.ValidateUser(user))
            {
                return Unauthorized();
            }

            var tokenDto = await _service.AuthenticationService.CreateToken(true);

            var options = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.Now.AddDays(1),
                Secure = true,
            };

            HttpContext.Response.Cookies.Append("X-Access-Token", tokenDto.AccessToken, options);
            HttpContext.Response.Cookies.Append("X-Refresh-Token", tokenDto.RefreshToken, options);

            return Ok(tokenDto);
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Logout()
        {
            if (!(Request.Cookies.TryGetValue("X-Access-Token", out var accessToken) && Request.Cookies.TryGetValue("X-Refresh-Token", out var refreshToken)))
            {
                return BadRequest();
            }

            HttpContext.Response.Cookies.Delete("X-Access-Token");
            HttpContext.Response.Cookies.Delete("X-Refresh-Token");

            await _service.AuthenticationService.RevokeRefresh(new TokenDto(accessToken, refreshToken));

            return NoContent();
        }

        [HttpPost("verifyemail")]
        public async Task<IActionResult> ConfirmEmail([FromBody]VerifyEmailDto emailDto)
        {
            var result = await _service.AuthenticationService.VerifyEmailAsync(emailDto);

            if (result.Succeeded)
            {
                return NoContent();
            }

            return BadRequest();
        }

        [HttpPost("forgotpassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPassword)
        {
            await _service.AuthenticationService.ForgotPasswordAsync(forgotPassword.Email);

            return NoContent();
        }

        [HttpPost("resetpassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetDto)
        {
            var result = await _service.AuthenticationService.ResetPasswordAsync(resetDto);

            if (result.Succeeded)
            {
                return NoContent();
            }

            foreach(var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            return BadRequest(ModelState);
        }
    }
}
