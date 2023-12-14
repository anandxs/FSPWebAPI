﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared;
using Shared.DataTransferObjects;

namespace FSPWebAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api")]
    [Authorize(Roles = Constants.GLOBAL_ROLE_USER)]
    public class RolesController : ControllerBase
    {
        private readonly IServiceManager _service;

        public RolesController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet("projects/{projectId:guid}/roles")]
        public async Task<IActionResult> GetAllRolesForProject(Guid projectId)
        {
            var roles = await _service.RoleService.GetAllRolesForProjectAsync(projectId, false);

            return Ok(roles);
        }

        [HttpPost("projects/{projectId:guid}/roles")]
        public async Task<IActionResult> CreateRole(Guid projectId, [FromBody] RoleForCreationDto roleDto)
        {
            await _service.RoleService.CreateRoleAsync(projectId, roleDto, false);

            return StatusCode(201);
        }

        [HttpPut("roles/{roleId:guid}")]
        public async Task<IActionResult> UpdateRole(Guid roleId)
        {
            return NoContent();
        }

        [HttpDelete("projects/{projectId:guid}/roles/{roleId:guid}")]
        public async Task<IActionResult> DeleteRole(Guid projectId, Guid roleId)
        {
            await _service.RoleService.DeleteRoleAsync(projectId, roleId, false);

            return NoContent();
        }
    }
}
