using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;
using System.Security.Claims;

namespace FSPWebAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api/users/{userId}/projects/{projectId}/members")]
    [Authorize]
    public class MemberController : ControllerBase
    {
        private readonly IServiceManager _service;

        public MemberController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetMembersForProject(string userId, Guid projectId)
        {
            var members = await _service.MemberService.GetProjectMembersAsync(userId, projectId, false);

            return Ok(members);
        }

        [HttpPost]
        public async Task<IActionResult> InviteUser(string userId, Guid projectId, [FromBody] MemberForCreationDto memberDto)
        {
            var requesterId = GetRequesterId();

            await _service.MemberService.InviteUserAsync(requesterId, userId, projectId, memberDto, false);

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
