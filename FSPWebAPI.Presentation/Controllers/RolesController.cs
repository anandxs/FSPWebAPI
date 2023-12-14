using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared;
using Shared.DataTransferObjects;

namespace FSPWebAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api/projects/{projectId:guid}/roles")]
    [Authorize(Roles = Constants.GLOBAL_ROLE_USER)]
    public class RolesController : ControllerBase
    {
        private readonly IServiceManager _service;

        public RolesController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRolesForProject(Guid projectId)
        {
            var roles = await _service.RoleService.GetAllRolesForProjectAsync(projectId, false);

            return Ok(roles);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(Guid projectId, [FromBody] RoleForCreationDto roleDto)
        {
            await _service.RoleService.CreateRoleAsync(projectId, roleDto, false);

            return StatusCode(201);
        }

        [HttpPut("{roleId:guid}")]
        public async Task<IActionResult> UpdateRole(Guid projectId, Guid roleId, [FromBody] RoleForUpdateDto roleDto)
        {
            await _service.RoleService.UpdateRoleAsync(projectId, roleId, roleDto,true);

            return NoContent();
        }

        [HttpDelete("{roleId:guid}")]
        public async Task<IActionResult> DeleteRole(Guid projectId, Guid roleId)
        {
            await _service.RoleService.DeleteRoleAsync(projectId, roleId, false);

            return NoContent();
        }
    }
}
