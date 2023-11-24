using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;
using System.Security.Claims;

namespace FSPWebAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api/profile")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IServiceManager _services;

        public UserController(IServiceManager services)
        {
            _services = services;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserInfo()
        {
            var requesterId = GetRequesterId();

            var user = await _services.UserService.GetUser(requesterId);

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUserInfo([FromBody] UserForUpdateDto userDto)
        {
            var requesterId = GetRequesterId();

            await _services.UserService.UpdateUser(requesterId, userDto);

            return NoContent();
        }

        private string GetRequesterId()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity!;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            return claim!.Value;
        }
    }
}
