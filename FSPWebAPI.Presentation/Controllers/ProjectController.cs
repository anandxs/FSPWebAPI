using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

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

        [HttpGet("{projectId:guid}", Name = "GetProjectById")]
        public async Task<IActionResult> GetProjectOwnedByUser(string userId, Guid projectId)
        {
            var project = await _service.ProjectService.GetProjectOwnedByUserAsync(userId, projectId, false);

            return Ok(project);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject(string userId, [FromBody] ProjectForCreationDto projectDto)
        {
            if (projectDto is null)
            {
                return BadRequest("ProjectDto is null.");
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            var project = await _service.ProjectService.CreateProjectAsync(userId, projectDto);

            return CreatedAtRoute("GetProjectById", new { userId = userId, projectId = project.ProjectId }, project);
        }

        [HttpPut("{projectId:guid}")]
        public async Task<IActionResult> UpdateProject(string userId, Guid projectId, [FromBody] ProjectForUpdateDto projectDto)
        {
            if (projectDto is null)
            {
                return BadRequest("ProjectDto is null.");
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            await _service.ProjectService.UpdateProject(userId, projectId, projectDto, true);

            return NoContent();
        }

        [HttpPut("{projectId:guid}/archive")]
        public async Task<IActionResult> ToggleArchiveStatus(string userId, Guid projectId)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            await _service.ProjectService.ToggleArchive(userId, projectId, true);

            return NoContent();
        }

        [HttpDelete("{projectId:guid}")]
        public async Task<IActionResult> DeleteProject(string userId, Guid projectId)
        {
            await _service.ProjectService.DeleteProject(userId, projectId, false);

            return NoContent();
        }
    }
}
