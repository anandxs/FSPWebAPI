using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace FSPWebAPI.Presentation.Controllers
{
    [Route("api/superadmin")]
    [ApiController]
    public class SuperAdminController : ControllerBase
    {
        private readonly IServiceManager _service;

        public SuperAdminController(IServiceManager service)
        {
            _service = service;
        }

        #region DEFAULT PROJECT ROLE MANAGEMENT

        [HttpGet("roles")]
        public IActionResult GetProjectRoles()
        {
            return Ok();
        }

        [HttpPost("roles")]
        public IActionResult CreateProjectRole()
        {
            return NoContent();
        }

        [HttpPut("roles/{roleId:guid}")]
        public IActionResult UpdateProjectRole(Guid roleId)
        {
            return NoContent();
        }

        [HttpDelete("roles/{roleId:guid}")]
        public IActionResult DeleteProjectRole(Guid roleId)
        {
            return NoContent();
        }

        #endregion
    }
}
