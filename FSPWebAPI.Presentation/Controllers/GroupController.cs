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

        // get all lists for project

        [HttpGet("{groupId}", Name = "GetGroupById")]
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

        // update list

        // toggle archive status for list

        // delete list
    }
}
