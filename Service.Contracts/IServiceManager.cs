﻿namespace Service.Contracts
{
    public interface IServiceManager
    {
        IProjectService ProjectService { get; }
        IRoleService RoleService { get; }
        IStageService StageService { get; }
        ITaskTypeService TaskTypeService { get; }
        ITagService TagService { get; }
        ICardService CardService { get; }
        IAuthenticationService AuthenticationService { get; }
        IUserService UserService { get; }
        IMemberService MemberService { get; }
    }
}
