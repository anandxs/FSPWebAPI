namespace FSPWebAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api/projects/{projectId:guid}/tasks/{taskId:guid}/comments")]
    [Authorize(Roles = Constants.GLOBAL_ROLE_USER)]
    public class CommentController : ControllerBase
    {
        private readonly IServiceManager _service;

        public CommentController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCommentsForTask(Guid projectId, Guid taskId)
        {
            var comments = await _service.CommentService.GetAllCommentsForTaskAsync(projectId, taskId, false);

            return Ok(comments);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> AddCommentToTask(Guid projectId, Guid taskId, [FromBody] TaskCommentForCreationDto commentDto)
        {
            await _service.CommentService.AddCommentToTaskAsync(projectId, taskId, commentDto, false);

            return NoContent();
        }
    }
}
