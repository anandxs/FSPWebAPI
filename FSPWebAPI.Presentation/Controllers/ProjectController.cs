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
    [Route("api/owner/{ownerId}/projects")]
    [Authorize(Roles = Constants.USER_ROLE)]
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
        public async Task<IActionResult> GetProjectUserIsPartOf(string ownerId, Guid projectId)
        {
            var requesterId = GetRequesterId();

            var project = await _service.ProjectService.GetProjectUserIsPartOfAsync(ownerId, projectId, requesterId, false);

            return Ok(project);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateProject(string ownerId, [FromBody] ProjectForCreationDto projectDto)
        {
            var requesterId = GetRequesterId();

            var project = await _service.ProjectService.CreateProjectAsync(ownerId, requesterId, projectDto);

            return CreatedAtRoute("GetProjectById", new { ownerId = ownerId, projectId = project.ProjectId }, project);
        }

        [HttpPut("{projectId:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateProject(string ownerId, Guid projectId, [FromBody] ProjectForUpdateDto projectDto)
        {
            var requesterId = GetRequesterId();

            await _service.ProjectService.UpdateProjectAsync(ownerId, projectId, requesterId, projectDto, true);

            return NoContent();
        }

        [HttpPut("{projectId:guid}/archive")]
        public async Task<IActionResult> ToggleArchiveStatus(string ownerId, Guid projectId)
        {
            var requesterId = GetRequesterId();

            await _service.ProjectService.ToggleArchive(ownerId, projectId, requesterId, true);

            return NoContent();
        }

        [HttpDelete("{projectId:guid}")]
        public async Task<IActionResult> DeleteProject(string ownerId, Guid projectId)
        {
            var requesterId = GetRequesterId();

            await _service.ProjectService.DeleteProject(ownerId, projectId, requesterId, false);

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
