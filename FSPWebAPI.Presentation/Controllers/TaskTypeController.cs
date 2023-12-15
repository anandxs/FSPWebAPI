using FSPWebAPI.Presentation.ActionFilters;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace FSPWebAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api")]
    public class TaskTypeController : ControllerBase
    {
        private readonly IServiceManager _service;

        public TaskTypeController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet("projects/{projectId:guid}/tasktypes")]
        public async Task<IActionResult> GetAllTasksForProject(Guid projectId)
        {
            var types = await _service.TaskTypeService.GetAllTaskTypesForProjectAsync(projectId, false);

            return Ok(types);
        }

        [HttpGet("tasktypes/{tasktypeId:guid}", Name = "GetTaskTypeById")]
        public async Task<IActionResult> GetTaskTypeById(Guid tasktypeId)
        {
            var type = await _service.TaskTypeService.GetTaskTypeByIdAsync(tasktypeId, false);

            return Ok(type);
        }

        [HttpPost("projects/{projectId:guid}/tasktypes")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateTaskType(Guid projectId, [FromBody] TaskTypeForCreationDto taskTypeForCreationDto)
        {
            var type = await _service.TaskTypeService.CreateTaskTypeAsync(projectId, taskTypeForCreationDto, false);

            return CreatedAtRoute(
                "GetTaskTypeById",
                new
                {
                    tasktypeId = type.TypeId
                },
                type);
        }

        [HttpPut("tasktypes/{tasktypeId:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateTaskType(Guid tasktypeId, [FromBody] TaskTypeForUpdateDto taskTypeForUpdate)
        {
            await _service.TaskTypeService.UpdateTaskTypeAsync(tasktypeId, taskTypeForUpdate, true);

            return NoContent();
        }

        [HttpPut("tasktypes/{tasktypeId:guid}/archive")]
        public async Task<IActionResult> ToggleTaskTypeArchiveStatus(Guid tasktypeId)
        {
            await _service.TaskTypeService.ToggleTaskTypeArchiveStatusAsync(tasktypeId, true);

            return NoContent();
        }

        [HttpDelete("tasktypes/{tasktypeId:guid}")]
        public async Task<IActionResult> DeleteTaskType(Guid tasktypeId)
        {
            await _service.TaskTypeService.DeleteTaskTypeAsync(tasktypeId, false);

            return NoContent();
        }
    }
}
