using System.Security.Claims;

namespace FSPWebAPI.Presentation.Controllers;

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
        var members = await _service.MemberService.GetAllProjectMembersAsync(projectId, false);

        return Ok(members);
    }

    [HttpGet("{memberId}")]
    public async Task<IActionResult> GetProjectMember(Guid projectId, string memberId)
    {
        var member = await _service.MemberService.GetProjectMemberAsync(projectId, memberId, false);

        return Ok(member);
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> AddMember(Guid projectId, [FromBody] MemberForCreationDto memberDto)
    {
        var result = await _service.MemberService.AddMemberAsync(projectId, memberDto, false);

        return Ok(new { message = result });
    }

    [HttpPost("accept")]
    public async Task<IActionResult> AcceptInvite(Guid projectId)
    {
        var result = await _service.MemberService.AcceptInviteAsync(projectId);

        if (result)
            return Ok(new { message = "Successfully joined project"});

        return BadRequest(new { message = "Invalid invitation link."});
    }

    [HttpPut]
    public async Task<IActionResult> ChangeMemberRole(Guid projectId, [FromBody] MemberForUpdateDto memberDto)
    {
        await _service.MemberService.ChangeMemberRoleAsync(projectId, memberDto, true);

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

        await _service.MemberService.RemoveMemberAsync(projectId, memberId, false);

        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> ExitProject(Guid projectId)
    {
        await _service.MemberService.ExitProjectAsync(projectId, false);

        return NoContent();
    }

    private string GetRequesterId()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity!;
        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

        return claim!.Value;
    }
}
