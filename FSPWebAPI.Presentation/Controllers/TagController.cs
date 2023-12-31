namespace FSPWebAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api/projects/{projectId:guid}/tags")]
    public class TagController :  ControllerBase
    {
        private readonly IServiceManager _service;

        public TagController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTagsForProject(Guid projectId)
        {
            var tags = await _service.TagService.GetAllTagsForProjectAsync(projectId, false);

            return Ok(tags);
        }

        [HttpGet("{tagId:guid}", Name = "GetTagById")]
        public async Task<IActionResult> GetTagById(Guid projectId, Guid tagId)
        {
            var tag = await _service.TagService.GetTagByIdAsync(projectId, tagId, false);

            return Ok(tag);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateTag(Guid projectId, [FromBody] TagForCreationDto tagForCreationDto)
        {
            var tag = await _service.TagService.CreateTagAsync(projectId, tagForCreationDto, false);

            return CreatedAtRoute(
                "GetTagById",
                new
                {
                    projectId,
                    tagId = tag.TagId
                },
                tag);
        }

        [HttpPut("{tagId:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateTag(Guid projectId, Guid tagId, [FromBody] TagForUpdateDto tagForUpdateDto)
        {
            await _service.TagService.UpdateTagAsync(projectId, tagId, tagForUpdateDto, true);

            return NoContent();
        }

        [HttpDelete("{tagId:guid}")]
        public async Task<IActionResult> DeleteTag(Guid projectId, Guid tagId)
        {
            await _service.TagService.DeleteTagAsync(projectId, tagId, false);

            return NoContent();
        }
    }
}
