using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace FSPWebAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api/users/{userId}/projects/{projectId}/roles")]
    [Authorize]
    public class RoleController : ControllerBase
    {
        private readonly IServiceManager _service;

        public RoleController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetProjectRoles(string userId, Guid projectId)
        {
            var roles = await _service.RoleService.GetProjectRolesAsync(userId, projectId, false);

            return Ok(roles);
        }
    }
}
