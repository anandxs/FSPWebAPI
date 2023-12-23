using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace FSPWebAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api/projects/{projectId:guid}/tasks/{taskId:guid}/attachments")]
    public class AttachmentController : ControllerBase
    {
        private readonly IServiceManager _service;

        public AttachmentController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTaskAttachments(Guid projectId, Guid taskId)
        {
            var results = await _service.AttachmentService.GetAllProjectAttachmentsAsync(projectId, taskId, false);

            return Ok(results);
        }

        [HttpGet("{attachmentId:guid}")]
        public async Task<IActionResult> GetAttachmentById(Guid projectId, Guid taskId, Guid attachmentId)
        {
            var result = await _service.AttachmentService.GetAttachmentByIdAsync(projectId, taskId, attachmentId, false);

            return Ok(result);
        }
    }
}
