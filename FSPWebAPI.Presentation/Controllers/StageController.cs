namespace FSPWebAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api/projects/{projectId:guid}/stages")]
    [Authorize(Roles = Constants.GLOBAL_ROLE_USER)]
    public class StageController : ControllerBase
    {
        private readonly IServiceManager _service;

        public StageController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetStagesForProject(Guid projectId)
        {
            var stages = await _service.StageService.GetAllStagesForProjectAsync(projectId, false);

            return Ok(stages);
        }

        [HttpGet("{stageId:guid}", Name = "GetStageById")]
        public async Task<IActionResult> GetStageById(Guid projectId, Guid stageId)
        {
            var stage = await _service.StageService.GetStageByIdAsync(projectId, stageId, false);

            return Ok(stage);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateStage(Guid projectId, [FromBody] StageForCreationDto stageForCreation)
        {
            var stage = await _service.StageService.CreateStageAsync(projectId, stageForCreation, false);

            return CreatedAtRoute(
                "GetStageById", 
                new 
                { 
                    projectId = projectId,
                    stageId = stage.StageId
                }, 
                stage);
        }

        [HttpPut("{stageId:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateStage(Guid projectId, Guid stageId, [FromBody] StageForUpdateDto stageForUpdate)
        {
            await _service.StageService.UpdateStageAsync(projectId, stageId, stageForUpdate, true);

            return NoContent();
        }

        [HttpPut("{stageId:guid}/archive")]
        public async Task<IActionResult> ToggleStageArchiveStatus(Guid projectId, Guid stageId)
        {
            await _service.StageService.ToggleStageArchiveStatusAsync(projectId, stageId, true);

            return NoContent();
        }

        [HttpDelete("{stageId:guid}")]
        public async Task<IActionResult> DeleteStage(Guid projectId, Guid stageId)
        {
            await _service.StageService.DeleteStageAsync(projectId, stageId, false);

            return NoContent();
        }
    }
}
