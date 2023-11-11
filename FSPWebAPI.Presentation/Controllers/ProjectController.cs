using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace FSPWebAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api/users/{userId}/projects")]
    public class ProjectController : ControllerBase
    {
        private readonly IServiceManager _service;

        public ProjectController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetProjectsOwnedByUser(string userId)
        {
            var projects = await _service.ProjectService.GetProjectsOwnedByUserAsync(userId, false);

            return Ok(projects);
        }
    }
}
