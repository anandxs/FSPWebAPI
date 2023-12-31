using Microsoft.AspNetCore.Http;

namespace FSPWebAPI.Presentation.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IServiceManager _service;
        private readonly ITokenManager _tokenManager;

        public AuthenticationController(IServiceManager service, ITokenManager tokenManager)
        {
            _service = service;
            _tokenManager = tokenManager;
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

            return Ok(tokenDto);
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Logout(TokenDto tokenDto)
        {
            await _service.AuthenticationService.RevokeRefresh(tokenDto);
            await _tokenManager.DeactivateCurrentAsync();

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
