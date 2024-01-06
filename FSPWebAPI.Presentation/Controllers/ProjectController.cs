namespace FSPWebAPI.Presentation.Controllers;

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
        var projects = await _service.ProjectService.GetProjectsUserIsPartOfAsync(false);

        return Ok(projects);
    }

    [HttpGet("{projectId:guid}", Name = "GetProjectById")]
    public async Task<IActionResult> GetProjectUserIsPartOf(Guid projectId)
    {
        var project = await _service.ProjectService.GetProjectUserIsPartOfAsync(projectId, false);

        return Ok(project);
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> CreateProject([FromBody] ProjectForCreationDto projectDto)
    {
        var project = await _service.ProjectService.CreateProjectAsync(projectDto);

        return CreatedAtRoute("GetProjectById", new { projectId = project.ProjectId }, project);
    }

    [HttpPut("{projectId:guid}")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> UpdateProject(Guid projectId, [FromBody] ProjectForUpdateDto projectDto)
    {
        await _service.ProjectService.UpdateProjectAsync(projectId, projectDto, true);

        return NoContent();
    }

    [HttpPut("{projectId:guid}/archive")]
    public async Task<IActionResult> ToggleArchiveStatus(Guid projectId)
    {
        await _service.ProjectService.ToggleArchive(projectId, true);

        return NoContent();
    }

    [HttpDelete("{projectId:guid}")]
    public async Task<IActionResult> DeleteProject(Guid projectId)
    {
        await _service.ProjectService.DeleteProject(projectId, false);

        return NoContent();
    }
}
