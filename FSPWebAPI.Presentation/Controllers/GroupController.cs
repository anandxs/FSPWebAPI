using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace FSPWebAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api/users/{userId}/projects/{projectId}/groups")]
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
            var groups = await _service.GroupService.GetGroupsForProjectAsync(userId, projectId, false);

            return Ok(groups);
        }

        [HttpGet("{groupId:guid}", Name = "GetGroupById")]
        public async Task<IActionResult> GetGroupById(string userId, Guid projectId, Guid groupId)
        {
            var group = await _service.GroupService.GetGroupByIdAsync(userId, projectId, groupId, false);

            return Ok(group);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGroup(string userId, Guid projectId, [FromBody] GroupForCreationDto groupForCreation)
        {
            if (groupForCreation is null)
            {
                return BadRequest("GroupForCreationDto was null");
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            var group = await _service.GroupService.CreateGroupAsync(userId, projectId, groupForCreation, false);

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
        public async Task<IActionResult> UpdateGroup(string userId, Guid projectId, Guid groupId, [FromBody] GroupForUpdateDto groupForUpdate)
        {
            if (groupForUpdate is null)
            {
                return BadRequest("GroupForUpdateDto is null");
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            await _service.GroupService.UpdateGroupAsync(userId, projectId, groupId, groupForUpdate, true);

            return NoContent();
        }

        [HttpPut("{groupId:guid}/archive")]
        public async Task<IActionResult> ToggleGroupArchiveStatus(string userId, Guid projectId, Guid groupId)
        {
            await _service.GroupService.ToggleArchive(userId, projectId, groupId, true);

            return NoContent();
        }

        // delete list
    }
}
