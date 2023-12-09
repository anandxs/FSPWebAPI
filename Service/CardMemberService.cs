using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Service.Contracts;
using Shared;
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

        public async Task<IEnumerable<ProjectUserDto>> GetAllCardAssigneesAsync(Guid projectId, Guid cardId, bool trackChanges)
        {
            var requesterId = GetRequesterId();

            var requester = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(projectId, requesterId, trackChanges);

            if (requester == null)
            {
                throw new NotAProjectMemberForbiddenRequestException();
            }

            var entites = await _repositoryManager.CardMemberRepository.GetAllCardAssigneesAsync(cardId, trackChanges);

            var members = entites.Select(e => e.Member);

            var membersDto = _mapper.Map<IEnumerable<ProjectUserDto>>(members);

            return membersDto;
        }

        public async Task<IEnumerable<CardDto>> GetAllAssignedCardsForMemberAsync(Guid projectId, string memberId, bool trackChanges)
        {
            var requesterId = GetRequesterId();

            var requester = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(projectId, requesterId, trackChanges);

            if (requester == null)
            {
                throw new NotAProjectMemberForbiddenRequestException();
            }

            var entites = await _repositoryManager.CardMemberRepository.GetAllAssignedCardsForMemberAsync(memberId, trackChanges);

            var cards = entites.Select(e => e.Card);

            var cardsDto = _mapper.Map<IEnumerable<CardDto>>(cards);

            return cardsDto;
        }

        public async Task AssignMemberToCardAsync(Guid projectId, Guid cardId, string memberId, bool trackChanges)
        {
            var requesterId = GetRequesterId();

            var requester = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(projectId, requesterId, trackChanges);

            if (requester == null)
            {
                throw new NotAProjectMemberForbiddenRequestException();
            }
            else if (requester.Role != Constants.PROJECT_ROLE_ADMIN)
            {
                throw new IncorrectRoleForbiddenRequestException();
            }

            var card = await _repositoryManager.CardRepository.GetCardByIdAsync(cardId, trackChanges);

            if (card == null)
            {
                throw new CardNotFoundException(cardId);
            }

            var member = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(projectId, memberId, trackChanges);

            if (member == null)
            {
                throw new Exception("Given user is not a member of this project.");
            }

            var existingAssignee = await _repositoryManager.CardMemberRepository.GetAssignedMemberAsync(cardId, memberId, trackChanges);

            if (existingAssignee != null)
            {
                throw new Exception("Member is already assigned to this card");
            }

            var entity = new CardMember
            {
                CardId = cardId,
                MemberId = memberId,
            };

            _repositoryManager.CardMemberRepository.AssignMemberToCard(entity);
            await _repositoryManager.SaveAsync();
        }

        public async Task UnassignMemberFromCardAsync(Guid projectId, Guid cardId, string memberId, bool trackChanges)
        {
            var requesterId = GetRequesterId();

            var requester = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(projectId, requesterId, trackChanges);

            if (requester == null)
            {
                throw new NotAProjectMemberForbiddenRequestException();
            }
            else if (requester.Role != Constants.PROJECT_ROLE_ADMIN)
            {
                throw new IncorrectRoleForbiddenRequestException();
            }

            var card = await _repositoryManager.CardRepository.GetCardByIdAsync(cardId, trackChanges);

            if (card == null)
            {
                throw new CardNotFoundException(cardId);
            }

            var existingAssignee = await _repositoryManager.CardMemberRepository.GetAssignedMemberAsync(cardId, memberId, trackChanges);

            if (existingAssignee == null)
            {
                throw new Exception("Member is not assigned to this card.");
            }

            _repositoryManager.CardMemberRepository.UnassignMemberFromCard(existingAssignee);
            await _repositoryManager.SaveAsync();
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
