using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

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

        // create list

        // update list

        // toggle archive status for list

        // delete list
    }
}
