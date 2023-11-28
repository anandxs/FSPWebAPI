using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace FSPWebAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api/users/{userId}/projects/{projectId}/roles")]
    [Authorize]
    public class RoleController : ControllerBase
    {
        private readonly IServiceManager _service;

        public RoleController(IServiceManager service)
        {
            _service = service;
        }
    }
}
