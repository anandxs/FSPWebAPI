using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared;
using Shared.DataTransferObjects;
using System.Security.Claims;

namespace FSPWebAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api/projects/{projectId}/members")]
    [Authorize(Roles = Constants.GLOBAL_ROLE_USER)]
    public class MemberController : ControllerBase
    {
        private readonly IServiceManager _service;

        public MemberController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetMembersForProject(Guid projectId)
        {
            var requesterId = GetRequesterId();

            var members = await _service.MemberService.GetAllProjectMembersAsync(projectId, requesterId, false);

            return Ok(members);
        }

        [HttpPost]
        public async Task<IActionResult> InviteUser(Guid projectId, [FromBody] MemberForCreationDto memberDto)
        {
            var requesterId = GetRequesterId();

            await _service.MemberService.AddMemberAsync(projectId, requesterId, memberDto, false);

            return NoContent();
        }

        [HttpDelete("{memberId}")]
        public async Task<IActionResult> RemoveMember(Guid projectId, string memberId)
        {
            var requesterId = GetRequesterId();

            if (requesterId == memberId)
            {
                return BadRequest("You cannot remove yourself.");
            }

            await _service.MemberService.RemoveMemberAsync(projectId, memberId, requesterId, false);

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
