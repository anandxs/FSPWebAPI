using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Service.Contracts;
using Shared;
using Shared.DataTransferObjects;
using System;
using System.Security.Claims;

namespace Service
{
    public class TagService : ITagService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _contextAccessor;

        public TagService(IRepositoryManager repositoryManager, ILoggerManager logger, IMapper mapper, IHttpContextAccessor contextAccessor)
        {
            _repositoryManager = repositoryManager;
            _logger = logger;
            _mapper = mapper;
            _contextAccessor = contextAccessor;
        }

        public async Task<IEnumerable<TagDto>> GetAllTagsForProjectAsync(Guid projectId, bool trackChanges)
        {
            var requesterId = GetRequesterId();

            var requester = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(projectId, requesterId, false);

            if (requester is null)
            {
                throw new NotAProjectMemberForbiddenRequestException();
            }

            var tags = await _repositoryManager.TagRepository.GetAllProjectTagsAsync(projectId, trackChanges);

            var tagsDto = _mapper.Map<IEnumerable<TagDto>>(tags);

            return tagsDto;
        }

        public async Task<TagDto> GetTagByIdAsync(Guid tagId, bool trackChanges)
        {
            var requesterId = GetRequesterId();

            var tag = await _repositoryManager.TagRepository.GetTagByIdAsync(tagId, trackChanges);

            if (tag == null)
            {
                throw new TagNotFoundException(tagId);
            }

            var requester = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(tag.ProjectId, requesterId, false);

            if (requester is null)
            {
                throw new NotAProjectMemberForbiddenRequestException();
            }

            var tagDto = _mapper.Map<TagDto>(tag);
            
            return tagDto;
        }

        public async Task<TagDto> CreateTagAsync(Guid projectId, TagForCreationDto tagForCreationDto, bool trackChanges)
        {
            var requesterId = GetRequesterId();

            var requester = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(projectId, requesterId, false);

            if (requester is null)
            {
                throw new NotAProjectMemberForbiddenRequestException();
            }
            else if (requester.Role.Name != Constants.PROJECT_ROLE_ADMIN)
            {
                throw new IncorrectRoleForbiddenRequestException();
            }

            var project = requester.Project;

            if (project is null)
            {
                throw new ProjectNotFoundException(projectId);
            }

            var tag = _mapper.Map<Tag>(tagForCreationDto);

            _repositoryManager.TagRepository.CreateTag(projectId, tag);
            await _repositoryManager.SaveAsync();

            var tagDto = _mapper.Map<TagDto>(tag);

            return tagDto;
        }

        public async Task UpdateTagAsync(Guid tagId, TagForUpdateDto tagForUpdateDto, bool trackChanges)
        {
            var requesterId = GetRequesterId();

            var tag = await _repositoryManager.TagRepository.GetTagByIdAsync(tagId, trackChanges);

            if (tag == null)
            {
                throw new TaskTypeNotFoundException(tagId);
            }

            var requester = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(tag.ProjectId, requesterId, false);

            if (requester is null)
            {
                throw new NotAProjectMemberForbiddenRequestException();
            }
            else if (requester.Role.Name != Constants.PROJECT_ROLE_ADMIN)
            {
                throw new IncorrectRoleForbiddenRequestException();
            }

            _mapper.Map(tagForUpdateDto, tag);
            await _repositoryManager.SaveAsync();
        }

        public async Task DeleteTagAsync(Guid tagId, bool trackChanges)
        {
            var requesterId = GetRequesterId();

            var tag = await _repositoryManager.TagRepository.GetTagByIdAsync(tagId, trackChanges);

            if (tag == null)
            {
                throw new TagNotFoundException(tagId);
            }

            var requester = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(tag.ProjectId, requesterId, false);

            if (requester is null)
            {
                throw new NotAProjectMemberForbiddenRequestException();
            }
            else if (requester.Role.Name != Constants.PROJECT_ROLE_ADMIN)
            {
                throw new IncorrectRoleForbiddenRequestException();
            }

            _repositoryManager.TagRepository.DeleteTag(tag);
            await _repositoryManager.SaveAsync();
        }

        private string GetRequesterId()
        {
            var claimsIdentity = (ClaimsIdentity)_contextAccessor.HttpContext.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            return claim!.Value;
        }
    }
}
