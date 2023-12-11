using FSPWebAPI.Presentation.ActionFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared;
using Shared.DataTransferObjects;
using System.Security.Claims;

namespace FSPWebAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api")]
    [Authorize(Roles = Constants.GLOBAL_ROLE_USER)]
    public class GroupController : ControllerBase
    {
        private readonly IServiceManager _service;

        public GroupController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet("projects/{projectId:guid}/groups")]
        public async Task<IActionResult> GetGroupsForProject(Guid projectId)
        {
            var requesterId = GetRequesterId();

            var groups = await _service.GroupService.GetAllGroupsForProjectAsync(projectId, requesterId, false);

            return Ok(groups);
        }

        [HttpGet("groups/{groupId:guid}", Name = "GetGroupById")]
        public async Task<IActionResult> GetGroupById(Guid groupId)
        {
            var requesterId = GetRequesterId();

            var group = await _service.GroupService.GetGroupByIdAsync(groupId, requesterId, false);

            return Ok(group);
        }

        [HttpPost("projects/{projectId:guid}/groups")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateGroup(Guid projectId, [FromBody] GroupForCreationDto groupForCreation)
        {
            var requesterId = GetRequesterId();

            var group = await _service.GroupService.CreateGroupAsync(projectId, requesterId, groupForCreation, false);

            return CreatedAtRoute(
                "GetGroupById", 
                new 
                { 
                    groupId = group.GroupId
                }, 
                group);
        }

        [HttpPut("groups/{groupId:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateGroup(Guid groupId, [FromBody] GroupForUpdateDto groupForUpdate)
        {
            var requesterId = GetRequesterId();

            await _service.GroupService.UpdateGroupAsync(groupId, requesterId, groupForUpdate, true);

            return NoContent();
        }

        [HttpPut("groups/{groupId:guid}/archive")]
        public async Task<IActionResult> ToggleGroupArchiveStatus(Guid groupId)
        {
            var requesterId = GetRequesterId();

            await _service.GroupService.ToggleArchiveAsync(groupId, requesterId, true);

            return NoContent();
        }

        [HttpDelete("groups/{groupId:guid}")]
        public async Task<IActionResult> DeleteGroup(Guid groupId)
        {
            var requesterId = GetRequesterId();

            await _service.GroupService.DeleteGroupAsync(groupId, requesterId, false);

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
