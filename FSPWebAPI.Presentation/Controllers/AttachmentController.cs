namespace FSPWebAPI.Presentation.Controllers;

[ApiController]
[Route("api/projects/{projectId:guid}/tasks/{taskId:guid}/attachments")]
[Authorize(Roles = Constants.GLOBAL_ROLE_USER)]
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
        var (stream, fileName) = await _service.AttachmentService.GetAttachmentByIdAsync(projectId, taskId, attachmentId, false);
        
        return File(stream, "application/octet-stream", fileName);
    }

    [HttpPost]
    public async Task<IActionResult> UploadAttachment(Guid projectId, Guid taskId)
    {
        if (Request.Form.Files.Count() == 0)
        {
            return BadRequest(new {message = "File is required."});
        }

        await _service.AttachmentService.AddAttachmentAsync(projectId, taskId, false);

        return NoContent();
    }

    [HttpDelete("{attachmentId:guid}")]
    public async Task<IActionResult> DeleteAttachmentById(Guid projectId, Guid taskId, Guid attachmentId)
    {
        await _service.AttachmentService.DeleteAttachmentAsync(projectId, taskId, attachmentId, false);

        return NoContent();
    }
}
