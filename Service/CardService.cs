using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service
{
    public class CardService : ICardService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public CardService(
            IRepositoryManager repositoryManager,
            ILoggerManager logger,
            IMapper mapper,
            UserManager<User> userManager)
        {
            _repositoryManager = repositoryManager;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<IEnumerable<CardDto>> GetCardsForProjectAsync(string userId, Guid projectId, string requesterId, bool trackChanges)
        {
            await CheckIfRequesterIsAuthorized(projectId, requesterId, new HashSet<string> { "Admin", "Member", "Observer" });

            await CheckIfUserAndProjectExists(userId, projectId, trackChanges);

            var groups = await _repositoryManager.GroupRepository.GetAllGroupsForProjectAsync(projectId, trackChanges);

            List<Card> cards = new List<Card>();

            foreach (var group in groups)
            {
                var temp = await _repositoryManager.CardRepository.GetCardsForGroupAsync(group.GroupId, trackChanges);

                cards.AddRange(temp);
            }

            var cardsDto = _mapper.Map<IEnumerable<CardDto>>(cards);

            return cardsDto;
        }

        
        public async Task<IEnumerable<CardDto>> GetCardsForGroupAsync(string userId, Guid projectId, string requesterId, Guid groupId, bool trackChanges)
        {
            await CheckIfRequesterIsAuthorized(projectId, requesterId, new HashSet<string> { "Admin", "Member", "Observer" });

            await CheckIfUserProjectAndGroupExistsAsync(userId, projectId, groupId, trackChanges);

            var cards = await _repositoryManager.CardRepository.GetCardsForGroupAsync(groupId, trackChanges);

            var cardsDto = _mapper.Map<IEnumerable<CardDto>>(cards);

            return cardsDto;
        }

        public async Task<CardDto> GetCardByIdAsync(Guid projectId, Guid cardId, string requesterId, bool trackChanges)
        {
            var requester = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(projectId, requesterId, false);

            if (requester is null)
            {
                throw new NotAProjectMemberForbiddenRequestException();
            }

            var card = await _repositoryManager.CardRepository.GetCardByIdAsync(cardId, trackChanges);

            if (card == null)
            {
                throw new CardNotFoundException(cardId);
            }

            var cardDto = _mapper.Map<CardDto>(card);

            return cardDto;
        }

        public async Task<(CardDto cardDto, Guid projectId)> CreateCardAsync(Guid groupId, string requesterId, CardForCreationDto cardForCreation, bool trackChanges)
        {
            var group = await _repositoryManager.GroupRepository.GetGroupByIdAsync(groupId, trackChanges);

            if (group  == null)
            {
                throw new GroupNotFoundException(groupId);
            }

            var requester = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(group.ProjectId, requesterId, false);

            if (requester is null)
            {
                throw new NotAProjectMemberForbiddenRequestException();
            }

            var card = _mapper.Map<Card>(cardForCreation);

            _repositoryManager.CardRepository.CreateCard(groupId, requesterId, card);
            await _repositoryManager.SaveAsync();

            var cardDto = _mapper.Map<CardDto>(card);

            return (cardDto, group.ProjectId);
        }

        public async Task DeleteCardAsync(Guid projectId, Guid cardId, string requesterId, bool trackChanges)
        {
            var requester = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(projectId, requesterId, false);

            if (requester is null)
            {
                throw new NotAProjectMemberForbiddenRequestException();
            }

            var card = await _repositoryManager.CardRepository.GetCardByIdAsync(cardId, trackChanges);

            if (card == null)
            {
                throw new CardNotFoundException(cardId);
            }

            _repositoryManager.CardRepository.DeleteCard(card);
            await _repositoryManager.SaveAsync();
        }

        public async Task ToggleArchiveStatusAsync(Guid projectId, Guid cardId, string requesterId, bool trackChanges)
        {
            var requester = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(projectId, requesterId, false);

            if (requester is null)
            {
                throw new NotAProjectMemberForbiddenRequestException();
            }

            var card = await _repositoryManager.CardRepository.GetCardByIdAsync(cardId, trackChanges);

            if (card == null)
            {
                throw new CardNotFoundException(cardId);
            }

            card.IsActive = !card.IsActive; // maybe move to repository layer
            await _repositoryManager.SaveAsync();
        }

        public async Task UpdateCardAsync(Guid cardId, string requesterId, CardForUpdateDto cardForUpdate, bool trackChanges)
        {
            var group = await _repositoryManager.GroupRepository.GetGroupByIdAsync(cardForUpdate.GroupId, trackChanges);

            if (group == null)
            {
                throw new GroupNotFoundException(cardForUpdate.GroupId);
            }

            var requester = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(group.ProjectId, requesterId, false);

            if (requester is null)
            {
                throw new NotAProjectMemberForbiddenRequestException();
            }

            var card = await _repositoryManager.CardRepository.GetCardByIdAsync(cardId, trackChanges);

            if (card is null)
            {
                throw new CardNotFoundException(cardId);
            }

            _mapper.Map(cardForUpdate, card);
            await _repositoryManager.SaveAsync();
        }

        #region HELPER METHODS

        private async Task CheckIfRequesterIsAuthorized(Guid projectId, string requesterId, HashSet<string> allowedRoles)
        {
            var requester = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(projectId, requesterId, false);

            if (requester is null)
            {
                throw new NotAProjectMemberForbiddenRequestException();
            }

            var requesterRole = await _repositoryManager.ProjectRoleRepository.GetProjectRoleById(projectId, (Guid)requester.ProjectRoleId, false);

            if (!allowedRoles.Contains(requesterRole.Name))
            {
                throw new IncorrectRoleForbiddenRequestException();
            }
        }

        private async Task<Card> GetCardAndCheckIfItExists(string userId, Guid projectId, Guid groupId, Guid cardId, bool trackChanges)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                throw new UserNotFoundException(userId);
            }

            var project = await _repositoryManager.ProjectRepository.GetProjectOwnedByUserAsync(userId, projectId, trackChanges);

            if (project is null)
            {
                throw new ProjectNotFoundException(projectId);
            }

            //var group = await _repositoryManager.GroupRepository.GetGroupByIdAsync(projectId, groupId, trackChanges);

            //if (group is null)
            //{
            //    throw new GroupNotFoundException(groupId);
            //}

            var card = await _repositoryManager.CardRepository.GetCardByIdAsync(cardId, trackChanges);

            if (card is null)
            {
                throw new CardNotFoundException(cardId);
            }

            return card;
        }

        private async Task CheckIfUserProjectAndGroupExistsAsync(string userId, Guid projectId, Guid groupId, bool trackChanges)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                throw new UserNotFoundException(userId);
            }

            var project = await _repositoryManager.ProjectRepository.GetProjectOwnedByUserAsync(userId, projectId, trackChanges);

            if (project is null)
            {
                throw new ProjectNotFoundException(projectId);
            }

            var group = await _repositoryManager.GroupRepository.GetGroupByIdAsync(groupId, trackChanges);

            if (group is null)
            {
                throw new GroupNotFoundException(groupId);
            }
        }

        private async Task CheckIfUserAndProjectExists(string userId, Guid projectId, bool trackChanges)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                throw new UserNotFoundException(userId);
            }

            var project = await _repositoryManager.ProjectRepository.GetProjectOwnedByUserAsync(userId, projectId, trackChanges);

            if (project is null)
            {
                throw new ProjectNotFoundException(projectId);
            }
        }

        #endregion
    }
}
