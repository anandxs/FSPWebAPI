using FSPWebAPI.Presentation.ActionFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;
using System.Security.Claims;

namespace FSPWebAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api/users/{userId}/projects/{projectId}/groups")]
    [Authorize]
    public class GroupController : ControllerBase
    {
        private readonly IServiceManager _service;

        public GroupController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetGroupsForProject(string userId, Guid projectId)
        {
            var requesterId = GetRequesterId();

            var groups = await _service.GroupService.GetGroupsForProjectAsync(userId, projectId, requesterId, false);

            return Ok(groups);
        }

        [HttpGet("{groupId:guid}", Name = "GetGroupById")]
        public async Task<IActionResult> GetGroupById(string userId, Guid projectId, Guid groupId)
        {
            var requesterId = GetRequesterId();

            var group = await _service.GroupService.GetGroupByIdAsync(userId, projectId, requesterId, groupId, false);

            return Ok(group);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateGroup(string userId, Guid projectId, [FromBody] GroupForCreationDto groupForCreation)
        {
            var requesterId = GetRequesterId();

            var group = await _service.GroupService.CreateGroupAsync(userId, projectId, requesterId, groupForCreation, false);

            return CreatedAtRoute(
                "GetGroupById", 
                new 
                { 
                    userId = userId,
                    projectId = projectId,
                    groupId = group.GroupId
                }, 
                group);
        }

        [HttpPut("{groupId:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateGroup(string userId, Guid projectId, Guid groupId, [FromBody] GroupForUpdateDto groupForUpdate)
        {
            var requesterId = GetRequesterId();

            await _service.GroupService.UpdateGroupAsync(userId, projectId, requesterId, groupId, groupForUpdate, true);

            return NoContent();
        }

        [HttpPut("{groupId:guid}/archive")]
        public async Task<IActionResult> ToggleGroupArchiveStatus(string userId, Guid projectId, Guid groupId)
        {
            var requesterId = GetRequesterId();

            await _service.GroupService.ToggleArchive(userId, projectId, requesterId, groupId, true);

            return NoContent();
        }

        [HttpDelete("{groupId:guid}")]
        public async Task<IActionResult> DeleteGroup(string userId, Guid projectId, Guid groupId)
        {
            var requesterId = GetRequesterId();

            await _service.GroupService.DeleteGroup(userId, projectId, requesterId, groupId, false);

            return NoContent();
        }

        private string GetRequesterId()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            return claim.Value;
        }
    }
}
