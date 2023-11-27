using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

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
            return Ok();
        }
    }
}
