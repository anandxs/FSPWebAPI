using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared;

namespace FSPWebAPI.Presentation.Controllers
{
    [Route("api/superadmin")]
    [ApiController]
    [Authorize(Roles = Constants.GLOBAL_ROLE_SUPERADMIN)]
    public class SuperAdminController : ControllerBase
    {
        private readonly IServiceManager _service;

        public SuperAdminController(IServiceManager service)
        {
            _service = service;
        }

        #region USER MANAGEMENT

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _service.UserService.GetUsersAsync(false);

            return Ok(users);
        }

        [HttpPut("users/{userId}")]
        public async Task<IActionResult> ToggleUserStatus(string userId)
        {
            await _service.UserService.ToggleUserAccountStatus(userId);

            return NoContent();
        }

        #endregion

        #region DEFAULT PROJECT ROLE MANAGEMENT

        //[HttpGet("roles")]
        //public async Task<IActionResult> GetProjectRoles()
        //{
        //    var roles = await _service.DefaultProjectRoleService.GetAllRolesAsync(false);

        //    return Ok(roles);
        //}

        //[HttpPost("roles")]
        //[ServiceFilter(typeof(ValidationFilterAttribute))]
        //public async Task<IActionResult> CreateProjectRole([FromBody] DefaultProjectRoleForCreationDto roleDto)
        //{
        //    await _service.DefaultProjectRoleService.CreateRoleAsync(roleDto, false);

        //    return StatusCode(201);
        //}

        //[HttpPut("roles/{roleId:guid}")]
        //public async Task<IActionResult> UpdateProjectRole(Guid roleId, [FromBody] DefaultProjectRoleForUpdateDto roleDto)
        //{
        //    await _service.DefaultProjectRoleService.UpdateRoleAsync(roleId, roleDto, true);

        //    return NoContent();
        //}

        //[HttpDelete("roles/{roleId:guid}")]
        //public async Task<IActionResult> DeleteProjectRole(Guid roleId)
        //{
        //    await _service.DefaultProjectRoleService.DeleteRoleAsync(roleId, false);

        //    return NoContent();
        //}

        #endregion
    }
}
