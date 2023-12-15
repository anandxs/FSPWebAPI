using FSPWebAPI.Presentation.ActionFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared;
using Shared.DataTransferObjects;
using System.Security.Claims;

namespace FSPWebAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api")]
    [Authorize(Roles = Constants.GLOBAL_ROLE_USER)]
    public class StageController : ControllerBase
    {
        private readonly IServiceManager _service;

        public StageController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet("projects/{projectId:guid}/stages")]
        public async Task<IActionResult> GetStagesForProject(Guid projectId)
        {
            var requesterId = GetRequesterId();

            var groups = await _service.StageService.GetAllStagesForProjectAsync(projectId, requesterId, false);

            return Ok(groups);
        }

        [HttpGet("stages/{stageId:guid}", Name = "GetStageById")]
        public async Task<IActionResult> GetStageById(Guid stageId)
        {
            var requesterId = GetRequesterId();

            var group = await _service.StageService.GetStageByIdAsync(stageId, requesterId, false);

            return Ok(group);
        }

        [HttpPost("projects/{projectId:guid}/stages")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateStage(Guid projectId, [FromBody] StageForCreationDto groupForCreation)
        {
            var requesterId = GetRequesterId();

            var stage = await _service.StageService.CreateStageAsync(projectId, requesterId, groupForCreation, false);

            return CreatedAtRoute(
                "GetStageById", 
                new 
                { 
                    stageId = stage.StageId
                }, 
                stage);
        }

        [HttpPut("stages/{stageId:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateStage(Guid stageId, [FromBody] StageForUpdateDto groupForUpdate)
        {
            var requesterId = GetRequesterId();

            await _service.StageService.UpdateStageAsync(stageId, requesterId, groupForUpdate, true);

            return NoContent();
        }

        [HttpPut("stages/{stageId:guid}/archive")]
        public async Task<IActionResult> ToggleStageArchiveStatus(Guid stageId)
        {
            var requesterId = GetRequesterId();

            await _service.StageService.ToggleStageArchiveStatusAsync(stageId, requesterId, true);

            return NoContent();
        }

        [HttpDelete("stages/{stageId:guid}")]
        public async Task<IActionResult> DeleteStage(Guid stageId)
        {
            var requesterId = GetRequesterId();

            await _service.StageService.DeleteStageAsync(stageId, requesterId, false);

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
