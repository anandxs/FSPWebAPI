using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using System.Security.Claims;

namespace FSPWebAPI.Presentation.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/projects/{projectId:guid}")]
    public class CardMemberController : ControllerBase
    {
        private readonly IServiceManager _service;

        public CardMemberController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet("cards/{cardId:guid}/assignees")]
        public async Task<IActionResult> GetAllCardAssignees(Guid projectId, Guid cardId)
        {
            var cards = await _service.CardMemberService.GetAllCardAssigneesAsync(projectId, cardId, false);

            return Ok(cards);
        }
    }
}
