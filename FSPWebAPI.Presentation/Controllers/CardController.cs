using FSPWebAPI.Presentation.ActionFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.Constants;
using Shared.DataTransferObjects;
using System.Security.Claims;

namespace FSPWebAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api/owner/{ownerId}/projects/{projectId}")]
    [Authorize(Roles = Constants.USER_ROLE)]
    public class CardController : ControllerBase
    {
        private readonly IServiceManager _service;

        public CardController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet("cards")]
        public async Task<IActionResult> GetCardsForProject(string ownerId, Guid projectId)
        {
            var requesterId = GetRequesterId();

            var cards = await _service.CardService.GetCardsForProjectAsync(ownerId, projectId, requesterId, false);

            return Ok(cards);
        }

        [HttpGet("groups/{groupId:guid}/cards")]
        public async Task<IActionResult> GetCardsForGroup(string ownerId, Guid projectId, Guid groupId)
        {
            var requesterId = GetRequesterId();

            var cards = await _service.CardService.GetCardsForGroupAsync(ownerId, projectId, requesterId, groupId, false);

            return Ok(cards);
        }

        [HttpGet("groups/{groupId:guid}/cards/{cardId:guid}", Name = "GetCardById")]
        public async Task<IActionResult> GetCardById(string ownerId, Guid projectId, Guid groupId, Guid cardId) 
        {
            var requesterId = GetRequesterId();

            var card = await _service.CardService.GetCardByIdAsync(ownerId, projectId, requesterId, groupId, cardId, false);

            return Ok(card);
        }

        [HttpPost("groups/{groupId:guid}/cards")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateCard(string ownerId, Guid projectId, Guid groupId, CardForCreationDto cardForCreation)
        {
            var requesterId = GetRequesterId();

            var card = await _service.CardService.CreateCardAsync(ownerId, projectId, requesterId, groupId, cardForCreation, false);

            return CreatedAtRoute(
                "GetCardById", 
                new 
                {
                    ownerId,
                    projectId,
                    groupId,
                    cardId = card.CardId,
                },
                card);
        }

        [HttpPut("groups/{groupId:guid}/cards/{cardId:guid}")]
        public async Task<IActionResult> UpdateCard(string ownerId, Guid projectId, Guid groupId, Guid cardId, CardForUpdateDto cardForUpdate)
        {
            var requesterId = GetRequesterId();

            await _service.CardService.UpdateCardAsync(ownerId, projectId, requesterId, groupId, cardId, cardForUpdate, true);

            return NoContent();
        }

        [HttpPut("groups/{groupId:guid}/cards/{cardId:guid}/archive")]
        public async Task<IActionResult> ToggleArchiveStatus(string ownerId, Guid projectId, Guid groupId, Guid cardId)
        {
            var requesterId = GetRequesterId();

            await _service.CardService.ToggleArchiveStatusAsync(ownerId, projectId, requesterId, groupId, cardId, true);

            return NoContent();
        }

        [HttpDelete("groups/{groupId:guid}/cards/{cardId:guid}")]
        public async Task<IActionResult> DeleteCard(string ownerId, Guid projectId, Guid groupId, Guid cardId)
        {
            var requesterId = GetRequesterId();

            await _service.CardService.DeleteCardAsync(ownerId, projectId, requesterId, groupId, cardId, false);

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
