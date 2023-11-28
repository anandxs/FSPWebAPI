using FSPWebAPI.Presentation.ActionFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;
using System.Security.Claims;

namespace FSPWebAPI.Presentation.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/users/{userId}/projects")]
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
        public async Task<IActionResult> GetProjectUserIsPartOf(string userId, Guid projectId)
        {
            var requesterId = GetRequesterId();

            var project = await _service.ProjectService.GetProjectUserIsPartOfAsync(userId, projectId, requesterId, false);

            return Ok(project);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateProject(string userId, [FromBody] ProjectForCreationDto projectDto)
        {
            var requesterId = GetRequesterId();

            var project = await _service.ProjectService.CreateProjectAsync(userId, requesterId, projectDto);

            return CreatedAtRoute("GetProjectById", new { userId = userId, projectId = project.ProjectId }, project);
        }

        [HttpPut("{projectId:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateProject(string userId, Guid projectId, [FromBody] ProjectForUpdateDto projectDto)
        {
            var requesterId = GetRequesterId();

            await _service.ProjectService.UpdateProjectAsync(userId, projectId, requesterId, projectDto, true);

            return NoContent();
        }

        [HttpPut("{projectId:guid}/archive")]
        public async Task<IActionResult> ToggleArchiveStatus(string userId, Guid projectId)
        {
            var requesterId = GetRequesterId();

            await _service.ProjectService.ToggleArchive(userId, projectId, requesterId, true);

            return NoContent();
        }

        [HttpDelete("{projectId:guid}")]
        public async Task<IActionResult> DeleteProject(string userId, Guid projectId)
        {
            var requesterId = GetRequesterId();

            await _service.ProjectService.DeleteProject(userId, projectId, requesterId, false);

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
