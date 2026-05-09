using AutoMapper;
using final_task.DTOs;
using final_task.DTOs.Authitification;
using final_task.DTOs.TaskDto;
using final_task.DTOs.TasksDto;
using final_task.Models;

namespace final_task.AutoMappers
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<RegisterDto, User>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();


            CreateMap<CreateRoleDto,  Role>().ReverseMap();
            CreateMap<Role, RoleDto>().ReverseMap();
            CreateMap<UpdateRoleDto, Role>().ReverseMap();


            CreateMap<CreateProjectDto, Project>().ReverseMap();
            CreateMap<UpdateProjectDto, Project>().ReverseMap();
            CreateMap<Project, ProjectDto>().ReverseMap();
            CreateMap<Project, ProjectDto>()
            .ForMember(dest => dest.Members,
               opt => opt.MapFrom(src => src.ProjectMembers.Select(pm => pm.User)));

            CreateMap<CreateTaskDto, TasksProject>();
            CreateMap<UpdateTaskDto, TasksProject>();
            CreateMap<TasksProject, TaskDto>();
        }
    }
}
