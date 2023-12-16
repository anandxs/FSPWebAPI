using FSPWebAPI.Presentation.ActionFilters;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace FSPWebAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api")]
    public class TagController :  ControllerBase
    {
        private readonly IServiceManager _service;

        public TagController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet("projects/{projectId:guid}/tags")]
        public async Task<IActionResult> GetAllTagsForProject(Guid projectId)
        {
            var tags = await _service.TagService.GetAllTagsForProjectAsync(projectId, false);

            return Ok(tags);
        }

        [HttpGet("tags/{tagId:guid}", Name = "GetTagById")]
        public async Task<IActionResult> GetTagById(Guid tagId)
        {
            var tag = await _service.TagService.GetTagByIdAsync(tagId, false);

            return Ok(tag);
        }

        [HttpPost("projects/{projectId:guid}/tags")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateTag(Guid projectId, [FromBody] TagForCreationDto tagForCreationDto)
        {
            var tag = await _service.TagService.CreateTagAsync(projectId, tagForCreationDto, false);

            return CreatedAtRoute(
                "GetTagById",
                new
                {
                    tagId = tag.TagId
                },
                tag);
        }

        [HttpPut("tags/{tagId:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateTag(Guid tagId, [FromBody] TagForUpdateDto tagForUpdateDto)
        {
            await _service.TagService.UpdateTagAsync(tagId, tagForUpdateDto, true);

            return NoContent();
        }

        [HttpDelete("tags/{tagId:guid}")]
        public async Task<IActionResult> DeleteTag(Guid tagId)
        {
            await _service.TagService.DeleteTagAsync(tagId, false);

            return NoContent();
        }
    }
}
