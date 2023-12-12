using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

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
            var assignees = await _service.CardMemberService.GetAllCardAssigneesAsync(projectId, cardId, false);

            return Ok(assignees);
        }

        [HttpGet("assignee/{assigneeId}/cards")]
        public async Task<IActionResult> GetAllAssignedCardsForMember(Guid projectId, string assigneeId)
        {
            var cards = await _service.CardMemberService.GetAllAssignedCardsForMemberAsync(projectId, assigneeId, false);

            return Ok(cards);
        }

        [HttpPost("cards/{cardId:guid}/assignees/{assigneeId}")]
        public async Task<IActionResult> AssignMemberToCard(Guid projectId, Guid cardId, string assigneeId)
        {
            await _service.CardMemberService.AssignMemberToCardAsync(projectId, cardId, assigneeId, false);

            return Ok("Card assigned to member successfully.");
        }

        [HttpDelete("cards/{cardId:guid}/assignees/{assigneeId}")]
        public async Task<IActionResult> UnassignCardFromMember(Guid projectId, Guid cardId, string assigneeId)
        {
            await _service.CardMemberService.UnassignMemberFromCardAsync(projectId, cardId, assigneeId, false);

            return NoContent();
        }
    }
}
