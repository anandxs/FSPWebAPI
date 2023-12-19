using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared;

namespace FSPWebAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api/projects/{projectId:guid}")]
    [Authorize(Roles = Constants.GLOBAL_ROLE_USER)]
    public class StatsController : ControllerBase
    {
        private readonly IServiceManager _service;

        public StatsController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet("tasksperstage")]
        public async Task<IActionResult> GetTasksPerStage(Guid projectId)
        {
            var data = await _service.StatsService.GetTasksPerStageAsync(projectId, false);

            return Ok(data);
        }
    }
}
