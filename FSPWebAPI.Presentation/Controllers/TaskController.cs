namespace FSPWebAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api/projects/{projectId:guid}/tasks")]
    [Authorize(Roles = Constants.GLOBAL_ROLE_USER)]
    public class TaskController : ControllerBase
    {
        private readonly IServiceManager _service;

        public TaskController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTasksForProject(Guid projectId)
        {
            var tasks = await _service.TaskService.GetAllTasksForProjectAsync(projectId, false);

            return Ok(tasks);
        }

        [HttpGet("{taskId:guid}", Name = "GetTaskbyId")]
        public async Task<IActionResult> GetTaskById(Guid projectId, Guid taskId)
        {
            var task = await _service.TaskService.GetTaskByIdAsync(projectId, taskId, false);

            return Ok(task);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateTask(Guid projectId, [FromBody] TaskForCreationDto taskDto)
        {
            var task = await _service.TaskService.CreateTaskAsync(projectId, taskDto, false);

            return CreatedAtRoute(
                "GetTaskById",
                new
                {
                    projectId = projectId,
                    taskId = task.TaskId
                },
                task);
        }

        [HttpPut("{taskId:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateTask(Guid projectId, Guid taskId,[FromBody] TaskForUpdateDto taskDto)
        {
            await _service.TaskService.UpdateTaskAsync(projectId, taskId,taskDto, true);

            return NoContent();
        }

        [HttpDelete("{taskId:guid}")]
        public async Task<IActionResult> DeleteTask(Guid projectId, Guid taskId)
        {
            await _service.TaskService.DeleteTaskAsync(projectId, taskId, false);

            return NoContent();
        }
    }
}
