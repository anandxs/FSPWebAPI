using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared;

namespace FSPWebAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api/owner/{ownerId}/projects/{projectId}/roles")]
    [Authorize(Roles = Constants.GLOBAL_ROLE_USER)]
    public class RoleController : ControllerBase
    {
        private readonly IServiceManager _service;

        public RoleController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetProjectRoles(string ownerId, Guid projectId)
        {
            var roles = await _service.RoleService.GetProjectRolesAsync(ownerId, projectId, false);

            return Ok(roles);
        }
    }
}
