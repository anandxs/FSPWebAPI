using AutoMapper;
using Entities.Models;
using Shared.DataTransferObjects;

namespace FSPWebAPI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserForRegistrationDto, User>();
            CreateMap<UserForUpdateDto, User>();
            CreateMap<UserForPasswordUpdate, User>();
            CreateMap<User, ProjectUserDto>();
            CreateMap<User, UserDto>();
            CreateMap<Project, ProjectDto>();
            CreateMap<ProjectForCreationDto, Project>();
            CreateMap<ProjectForUpdateDto, Project>();
            CreateMap<StageForCreationDto, Stage>();
            CreateMap<Stage, StageDto>();
            CreateMap<Stage, IncludedStageDto>();
            CreateMap<StageForUpdateDto, Stage>();
            CreateMap<TaskTypeForCreationDto, TaskType>();
            CreateMap<TaskType, TaskTypeDto>();
            CreateMap<TaskType, IncludedTaskTypeDto>();
            CreateMap<TaskTypeForUpdateDto, TaskType>();
            CreateMap<Tag, TagDto>();
            CreateMap<TagForCreationDto, Tag>();
            CreateMap<TagForUpdateDto, Tag>();
            CreateMap<ProjectTask, CardDto>();
            CreateMap<CardForCreationDto, ProjectTask>();
            CreateMap<CardForUpdateDto, ProjectTask>();
            CreateMap<ProjectMember, ProjectMemberDto>();
            CreateMap<Role, RoleDto>();
            CreateMap<RoleForCreationDto, Role>();
            CreateMap<RoleForUpdateDto, Role>();
        }
    }
}
