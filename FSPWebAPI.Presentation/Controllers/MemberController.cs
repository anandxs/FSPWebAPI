using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared;
using Shared.DataTransferObjects;
using System.Security.Claims;

namespace FSPWebAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api/owner/{ownerId}/projects/{projectId}/members")]
    [Authorize(Roles = Constants.GLOBAL_ROLE_USER)]
    public class MemberController : ControllerBase
    {
        private readonly IServiceManager _service;

        public MemberController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetMembersForProject(string ownerId, Guid projectId)
        {
            //var members = await _service.MemberService.GetProjectMembersAsync(ownerId, projectId, false);

            //return Ok(members);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> InviteUser(string ownerId, Guid projectId, [FromBody] MemberForCreationDto memberDto)
        {
            var requesterId = GetRequesterId();

            await _service.MemberService.InviteUserAsync(requesterId, ownerId, projectId, memberDto, false);

            return NoContent();
        }

        [HttpDelete("{memberId}")]
        public async Task<IActionResult> RemoveMember(string ownerId, Guid projectId, string memberId)
        {
            var requesterId = GetRequesterId();

            if (ownerId == memberId)
            {
                return BadRequest("Invalid action.");
            }

            await _service.MemberService.RemoveMemberAsync(requesterId, ownerId, projectId, memberId, false);

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
