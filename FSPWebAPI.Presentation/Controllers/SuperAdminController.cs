using FSPWebAPI.Presentation.ActionFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.Constants;
using Shared.DataTransferObjects;

namespace FSPWebAPI.Presentation.Controllers
{
    [Route("api/superadmin")]
    [ApiController]
    [Authorize(Roles = Constants.SUPERADMIN_ROLE)]
    public class SuperAdminController : ControllerBase
    {
        private readonly IServiceManager _service;

        public SuperAdminController(IServiceManager service)
        {
            _service = service;
        }

        #region DEFAULT PROJECT ROLE MANAGEMENT

        [HttpGet("roles")]
        public async Task<IActionResult> GetProjectRoles()
        {
            var roles = await _service.DefaultProjectRoleService.GetRolesAsync(false);

            return Ok(roles);
        }

        [HttpPost("roles")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateProjectRole([FromBody] DefaultProjectRoleForCreationDto roleDto)
        {
            await _service.DefaultProjectRoleService.CreateRole(roleDto, false);

            return StatusCode(201);
        }

        [HttpPut("roles/{roleId:guid}")]
        public async Task<IActionResult> UpdateProjectRole(Guid roleId, [FromBody] DefaultProjectRoleForUpdateDto roleDto)
        {
            await _service.DefaultProjectRoleService.UpdateRole(roleId, roleDto, true);

            return NoContent();
        }

        [HttpDelete("roles/{roleId:guid}")]
        public async Task<IActionResult> DeleteProjectRole(Guid roleId)
        {
            await _service.DefaultProjectRoleService.DeleteRole(roleId, false);

            return NoContent();
        }

        #endregion
    }
}
