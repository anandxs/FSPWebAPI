using FSPWebAPI.Presentation.ActionFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.Constants;
using Shared.DataTransferObjects;
using System.Security.Claims;

namespace FSPWebAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api/owner/{ownerId}/projects/{projectId}/groups")]
    [Authorize(Roles = Constants.USER_ROLE)]
    public class GroupController : ControllerBase
    {
        private readonly IServiceManager _service;

        public GroupController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetGroupsForProject(string ownerId, Guid projectId)
        {
            var requesterId = GetRequesterId();

            var groups = await _service.GroupService.GetGroupsForProjectAsync(ownerId, projectId, requesterId, false);

            return Ok(groups);
        }

        [HttpGet("{groupId:guid}", Name = "GetGroupById")]
        public async Task<IActionResult> GetGroupById(string ownerId, Guid projectId, Guid groupId)
        {
            var requesterId = GetRequesterId();

            var group = await _service.GroupService.GetGroupByIdAsync(ownerId, projectId, requesterId, groupId, false);

            return Ok(group);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateGroup(string ownerId, Guid projectId, [FromBody] GroupForCreationDto groupForCreation)
        {
            var requesterId = GetRequesterId();

            var group = await _service.GroupService.CreateGroupAsync(ownerId, projectId, requesterId, groupForCreation, false);

            return CreatedAtRoute(
                "GetGroupById", 
                new 
                { 
                    ownerId = ownerId,
                    projectId = projectId,
                    groupId = group.GroupId
                }, 
                group);
        }

        [HttpPut("{groupId:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateGroup(string ownerId, Guid projectId, Guid groupId, [FromBody] GroupForUpdateDto groupForUpdate)
        {
            var requesterId = GetRequesterId();

            await _service.GroupService.UpdateGroupAsync(ownerId, projectId, requesterId, groupId, groupForUpdate, true);

            return NoContent();
        }

        [HttpPut("{groupId:guid}/archive")]
        public async Task<IActionResult> ToggleGroupArchiveStatus(string ownerId, Guid projectId, Guid groupId)
        {
            var requesterId = GetRequesterId();

            await _service.GroupService.ToggleArchive(ownerId, projectId, requesterId, groupId, true);

            return NoContent();
        }

        [HttpDelete("{groupId:guid}")]
        public async Task<IActionResult> DeleteGroup(string ownerId, Guid projectId, Guid groupId)
        {
            var requesterId = GetRequesterId();

            await _service.GroupService.DeleteGroup(ownerId, projectId, requesterId, groupId, false);

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
