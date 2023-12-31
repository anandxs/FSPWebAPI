using System.Security.Claims;

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
            var requesterId = GetRequesterId();

            var stages = await _service.StageService.GetAllStagesForProjectAsync(projectId, requesterId, false);

            return Ok(stages);
        }

        [HttpGet("{stageId:guid}", Name = "GetStageById")]
        public async Task<IActionResult> GetStageById(Guid projectId, Guid stageId)
        {
            var requesterId = GetRequesterId();

            var stage = await _service.StageService.GetStageByIdAsync(projectId, stageId, requesterId, false);

            return Ok(stage);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateStage(Guid projectId, [FromBody] StageForCreationDto stageForCreation)
        {
            var requesterId = GetRequesterId();

            var stage = await _service.StageService.CreateStageAsync(projectId, requesterId, stageForCreation, false);

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
            var requesterId = GetRequesterId();

            await _service.StageService.UpdateStageAsync(projectId, stageId, requesterId, stageForUpdate, true);

            return NoContent();
        }

        [HttpPut("{stageId:guid}/archive")]
        public async Task<IActionResult> ToggleStageArchiveStatus(Guid projectId, Guid stageId)
        {
            var requesterId = GetRequesterId();

            await _service.StageService.ToggleStageArchiveStatusAsync(projectId, stageId, requesterId, true);

            return NoContent();
        }

        [HttpDelete("{stageId:guid}")]
        public async Task<IActionResult> DeleteStage(Guid projectId, Guid stageId)
        {
            var requesterId = GetRequesterId();

            await _service.StageService.DeleteStageAsync(projectId, stageId, requesterId, false);

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
