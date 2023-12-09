using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using System.Security.Claims;

namespace FSPWebAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api")]
    public class CardMemberController : ControllerBase
    {
        private readonly IServiceManager _service;

        public CardMemberController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet("cards/{cardId:guid}")]
        public async Task<IActionResult> GetAllCardAssignees(Guid cardId)
        {
            var requesterId = GetRequesterId();

            // give requeseterid, cardid

            return Ok();
        }

        private string GetRequesterId()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity!;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            return claim!.Value;
        }
    }
}
