namespace FSPWebAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api/projects/{projectId:guid}/types")]
    public class TaskTypeController : ControllerBase
    {
        private readonly IServiceManager _service;

        public TaskTypeController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTasksForProject(Guid projectId)
        {
            var types = await _service.TaskTypeService.GetAllTaskTypesForProjectAsync(projectId, false);

            return Ok(types);
        }

        [HttpGet("{typeId:guid}", Name = "GetTaskTypeById")]
        public async Task<IActionResult> GetTaskTypeById(Guid projectId, Guid typeId)
        {
            var type = await _service.TaskTypeService.GetTaskTypeByIdAsync(projectId, typeId, false);

            return Ok(type);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateTaskType(Guid projectId, [FromBody] TaskTypeForCreationDto taskTypeForCreationDto)
        {
            var type = await _service.TaskTypeService.CreateTaskTypeAsync(projectId, taskTypeForCreationDto, false);

            return CreatedAtRoute(
                "GetTaskTypeById",
                new
                {
                    projectId = projectId,
                    typeId= type.TypeId
                },
                type);
        }

        [HttpPut("{typeId:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateTaskType(Guid projectId, Guid typeId, [FromBody] TaskTypeForUpdateDto taskTypeForUpdate)
        {
            await _service.TaskTypeService.UpdateTaskTypeAsync(projectId, typeId, taskTypeForUpdate, true);

            return NoContent();
        }

        [HttpPut("{typeId:guid}/archive")]
        public async Task<IActionResult> ToggleTaskTypeArchiveStatus(Guid projectId, Guid typeId)
        {
            await _service.TaskTypeService.ToggleTaskTypeArchiveStatusAsync(projectId, typeId, true);

            return NoContent();
        }

        [HttpDelete("{typeId:guid}")]
        public async Task<IActionResult> DeleteTaskType(Guid projectId, Guid typeId)
        {
            await _service.TaskTypeService.DeleteTaskTypeAsync(projectId, typeId, false);

            return NoContent();
        }
    }
}
