using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Microsoft.AspNetCore.Http;
using Service.Contracts;
using Shared.DataTransferObjects;
using System.Security.Claims;

namespace Service
{
    public class CardMemberService : ICardMemberService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _contextAccessor;

        public CardMemberService(IRepositoryManager repositoryManager, ILoggerManager logger, IMapper mapper, IHttpContextAccessor contextAccessor)
        {
            _repositoryManager = repositoryManager;
            _logger = logger;
            _mapper = mapper;
            _contextAccessor = contextAccessor;
        }

        public async Task<IEnumerable<CardDto>> GetAllCardAssigneesAsync(Guid projectId, Guid cardId, bool trackChanges)
        {
            var requesterId = GetRequesterId();

            var requester = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(projectId, requesterId, trackChanges);

            if (requester == null)
            {
                throw new NotAProjectMemberForbiddenRequestException();
            }

            var entites = await _repositoryManager.CardMemberRepository.GetAllCardAssigneesAsync(cardId, trackChanges);

            var cards = entites.Select(e => e.Card);

            var cardDto = _mapper.Map<IEnumerable<CardDto>>(cards);

            return cardDto;
        }

        #region HELPER METHODS

        private string GetRequesterId()
        {
            var claimsIdentity = (ClaimsIdentity)_contextAccessor.HttpContext.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            return claim!.Value;
        }

        #endregion
    }
}
