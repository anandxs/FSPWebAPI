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
            CreateMap<GroupForCreationDto, Group>();
            CreateMap<Group, GroupDto>();
            CreateMap<Group, IncludedGroupDto>();
            CreateMap<GroupForUpdateDto, Group>();
            CreateMap<Card, CardDto>();
            CreateMap<CardForCreationDto, Card>();
            CreateMap<CardForUpdateDto, Card>();
            CreateMap<ProjectMember, ProjectMemberDto>();
            CreateMap<DefaultProjectRole, DefaultProjectRoleDto>();
            CreateMap<DefaultProjectRoleForCreationDto, DefaultProjectRole>();
            CreateMap<DefaultProjectRoleForUpdateDto, DefaultProjectRole>();
        }
    }
}
