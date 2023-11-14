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

        public async Task<IEnumerable<CardDto>> GetCardsForGroupAsync(string userId, Guid projectId, string requesterId, Guid groupId, bool trackChanges)
        {
            await CheckIfRequesterIsAuthorized(projectId, requesterId, new HashSet<string> { "Admin", "Member", "Observer" });

            await CheckIfUserProjectAndGroupExistsAsync(userId, projectId, groupId, trackChanges);

            var companies = await _repositoryManager.CardRepository.GetCardsForGroupAsync(groupId, trackChanges);

            var companiesDto = _mapper.Map<IEnumerable<CardDto>>(companies);

            return companiesDto;
        }

        public async Task<CardDto> GetCardByIdAsync(string userId, Guid projectId, string requesterId, Guid groupId, Guid cardId, bool trackChanges)
        {
            await CheckIfRequesterIsAuthorized(projectId, requesterId, new HashSet<string> { "Admin", "Member", "Observer" });
            
            var card = await GetCardAndCheckIfItExists(userId, projectId, groupId, cardId, trackChanges);

            var cardDto = _mapper.Map<CardDto>(card);

            return cardDto;
        }

        public async Task<CardDto> CreateCardAsync(string userId, Guid projectId, string requesterId, Guid groupId, CardForCreationDto cardForCreation, bool trackChanges)
        {
            await CheckIfRequesterIsAuthorized(projectId, requesterId, new HashSet<string> { "Admin", "Member" });

            await CheckIfUserProjectAndGroupExistsAsync(userId, projectId, groupId, trackChanges);

            var card = _mapper.Map<Card>(cardForCreation);

            _repositoryManager.CardRepository.CreateCard(groupId, requesterId, card);
            await _repositoryManager.SaveAsync();

            var cardForReturn = _mapper.Map<CardDto>(card);

            return cardForReturn;
        }

        public async Task DeleteCardAsync(string userId, Guid projectId, string requesterId, Guid groupId, Guid cardId, bool trackChanges)
        {
            await CheckIfRequesterIsAuthorized(projectId, requesterId, new HashSet<string> { "Admin", "Member" });

            var card = await GetCardAndCheckIfItExists(userId, projectId, groupId, cardId, trackChanges);

            _repositoryManager.CardRepository.DeleteCard(card);
            await _repositoryManager.SaveAsync();
        }

        public async Task ToggleArchiveStatusAsync(string userId, Guid projectId, string requesterId, Guid groupId, Guid cardId, bool trackChanges)
        {
            await CheckIfRequesterIsAuthorized(projectId, requesterId, new HashSet<string> { "Admin", "Member", });

            var card = await GetCardAndCheckIfItExists(userId, projectId, groupId, cardId, trackChanges);

            card.IsActive = !card.IsActive;
            await _repositoryManager.SaveAsync();
        }

        public async Task UpdateCardAsync(string userId, Guid projectId, string requesterId, Guid groupId, Guid cardId, CardForUpdateDto cardForUpdate, bool trackChanges)
        {
            await CheckIfRequesterIsAuthorized(projectId, requesterId, new HashSet<string> { "Admin", "Member", });

            var cardFromDb = await GetCardAndCheckIfItExists(userId, projectId, groupId, cardId, trackChanges);

            _mapper.Map(cardForUpdate, cardFromDb);
            await _repositoryManager.SaveAsync();
        }

        #region HELPER METHODS

        private async Task CheckIfRequesterIsAuthorized(Guid projectId, string requesterId, HashSet<string> allowedRoles)
        {
            var requester = await _repositoryManager.ProjectMemberRepository.GetProjectMember(projectId, requesterId, false);

            if (requester is null)
            {
                throw new NotAProjectMemberException();
            }

            var requesterRole = await _repositoryManager.ProjectRoleRepository.GetProjectRoleById(projectId, (Guid)requester.ProjectRoleId, false);

            if (!allowedRoles.Contains(requesterRole.Name))
            {
                throw new IncorrectRoleException();
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

            var group = await _repositoryManager.GroupRepository.GetGroupByIdAsync(projectId, groupId, trackChanges);

            if (group is null)
            {
                throw new GroupNotFoundException(groupId);
            }

            var card = await _repositoryManager.CardRepository.GetCardByIdAsync(groupId, cardId, trackChanges);

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

            var group = await _repositoryManager.GroupRepository.GetGroupByIdAsync(projectId, groupId, trackChanges);

            if (group is null)
            {
                throw new GroupNotFoundException(groupId);
            }
        }

        #endregion
    }
}
