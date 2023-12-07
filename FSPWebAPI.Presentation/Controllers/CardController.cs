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
    [Route("api")]
    [Authorize(Roles = Constants.USER_ROLE)]
    public class CardController : ControllerBase
    {
        private readonly IServiceManager _service;

        public CardController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet("projects/{projectId:guid}/cards")]
        public async Task<IActionResult> GetAllCardForProject(Guid projectId)
        {
            var requesterId = GetRequesterId();

            var cards = await _service.CardService.GetAllCardsForProjectAsync(projectId, requesterId, false);

            return Ok(cards);
        }

        [HttpGet("projects/{projectId}/cards/{cardId:guid}", Name = "GetCardById")]
        public async Task<IActionResult> GetCardById(Guid projectId, Guid cardId) 
        {
            var requesterId = GetRequesterId();

            var card = await _service.CardService.GetCardByIdAsync(projectId, cardId, requesterId, false);

            return Ok(card);
        }

        [HttpPost("groups/{groupId:guid}/cards")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateCard(Guid groupId, CardForCreationDto cardForCreation)
        {
            var requesterId = GetRequesterId();

            var result = await _service.CardService.CreateCardAsync(groupId, requesterId, cardForCreation, false);

            return CreatedAtRoute(
                "GetCardById", 
                new 
                {
                    projectId = result.projectId,
                    cardId = result.cardDto.CardId,
                },
                result);
        }

        [HttpPut("cards/{cardId:guid}")]
        public async Task<IActionResult> UpdateCard(Guid cardId, CardForUpdateDto cardForUpdate)
        {
            var requesterId = GetRequesterId();

            await _service.CardService.UpdateCardAsync(cardId, requesterId, cardForUpdate, true);

            return NoContent();
        }

        [HttpPut("projects/{projectId}/cards/{cardId:guid}/archive")]
        public async Task<IActionResult> ToggleArchiveStatus(Guid projectId, Guid cardId)
        {
            var requesterId = GetRequesterId();

            await _service.CardService.ToggleArchiveStatusAsync(projectId, cardId, requesterId, true);

            return NoContent();
        }

        [HttpDelete("projects/{projectId}/cards/{cardId:guid}")]
        public async Task<IActionResult> DeleteCard(Guid projectId, Guid cardId)
        {
            var requesterId = GetRequesterId();

            await _service.CardService.DeleteCardAsync(projectId, cardId, requesterId, false);

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
