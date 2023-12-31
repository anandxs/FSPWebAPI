using System.Security.Claims;

namespace FSPWebAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api/projects")]
    [Authorize(Roles = Constants.GLOBAL_ROLE_USER)]
    public class ProjectController : ControllerBase
    {
        private readonly IServiceManager _service;

        public ProjectController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetProjectsUserIsPartOf()
        {
            var requesterId = GetRequesterId();

            var projects = await _service.ProjectService.GetProjectsUserIsPartOfAsync(requesterId, false);

            return Ok(projects);
        }

        [HttpGet("{projectId:guid}", Name = "GetProjectById")]
        public async Task<IActionResult> GetProjectUserIsPartOf(Guid projectId)
        {
            var requesterId = GetRequesterId();

            var project = await _service.ProjectService.GetProjectUserIsPartOfAsync(requesterId, projectId, false);

            return Ok(project);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateProject([FromBody] ProjectForCreationDto projectDto)
        {
            var requesterId = GetRequesterId();

            var project = await _service.ProjectService.CreateProjectAsync(requesterId, projectDto);

            return CreatedAtRoute("GetProjectById", new { projectId = project.ProjectId }, project);
        }

        [HttpPut("{projectId:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateProject(Guid projectId, [FromBody] ProjectForUpdateDto projectDto)
        {
            var requesterId = GetRequesterId();

            await _service.ProjectService.UpdateProjectAsync(requesterId, projectId, projectDto, true);

            return NoContent();
        }

        [HttpPut("{projectId:guid}/archive")]
        public async Task<IActionResult> ToggleArchiveStatus(Guid projectId)
        {
            var requesterId = GetRequesterId();

            await _service.ProjectService.ToggleArchive(requesterId, projectId, true);

            return NoContent();
        }

        [HttpDelete("{projectId:guid}")]
        public async Task<IActionResult> DeleteProject(Guid projectId)
        {
            var requesterId = GetRequesterId();

            await _service.ProjectService.DeleteProject(requesterId, projectId, false);

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
