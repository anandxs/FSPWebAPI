using FSPWebAPI.Presentation.ActionFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;
using System.Security.Claims;

namespace FSPWebAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api/users/{userId}/projects/{projectId}/groups/{groupId}/cards")]
    [Authorize]
    public class CardController : ControllerBase
    {
        private readonly IServiceManager _service;

        public CardController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetCardsForGroup(string userId, Guid projectId, Guid groupId)
        {
            var requesterId = GetRequesterId();

            var cards = await _service.CardService.GetCardsForGroupAsync(userId, projectId, requesterId, groupId, false);

            return Ok(cards);
        }

        [HttpGet("{cardId:guid}", Name = "GetCardById")]
        public async Task<IActionResult> GetCardById(string userId, Guid projectId, Guid groupId, Guid cardId) 
        {
            var requesterId = GetRequesterId();

            var card = await _service.CardService.GetCardByIdAsync(userId, projectId, requesterId, groupId, cardId, false);

            return Ok(card);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateCard(string userId, Guid projectId, Guid groupId, CardForCreationDto cardForCreation)
        {
            var requesterId = GetRequesterId();

            var card = await _service.CardService.CreateCardAsync(userId, projectId, requesterId, groupId, cardForCreation, false);

            return CreatedAtRoute(
                "GetCardById", 
                new 
                {
                    userId,
                    projectId,
                    groupId,
                    cardId = card.CardId,
                },
                card);
        }

        [HttpPut("{cardId:guid}")]
        public async Task<IActionResult> UpdateCard(string userId, Guid projectId, Guid groupId, Guid cardId, CardForUpdateDto cardForUpdate)
        {
            var requesterId = GetRequesterId();

            await _service.CardService.UpdateCardAsync(userId, projectId, requesterId, groupId, cardId, cardForUpdate, true);

            return NoContent();
        }

        [HttpPut("{cardId:guid}/archive")]
        public async Task<IActionResult> ToggleArchiveStatus(string userId, Guid projectId, Guid groupId, Guid cardId)
        {
            var requesterId = GetRequesterId();

            await _service.CardService.ToggleArchiveStatusAsync(userId, projectId, requesterId, groupId, cardId, true);

            return NoContent();
        }

        [HttpDelete("{cardId:guid}")]
        public async Task<IActionResult> DeleteCard(string userId, Guid projectId, Guid groupId, Guid cardId)
        {
            var requesterId = GetRequesterId();

            await _service.CardService.DeleteCardAsync(userId, projectId, requesterId, groupId, cardId, false);

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
