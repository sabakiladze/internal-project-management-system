using AutoMapper;
using final_task.DTOs;
using final_task.DTOs.Authitification;
using final_task.Interfaces;
using final_task.Models;
using Microsoft.EntityFrameworkCore;

namespace final_task.Services
{
    public class ProjectService : IProjectService
    {
        private readonly UsersContext _context;
        private readonly IMapper _mapper;
        public ProjectService(UsersContext context, IMapper mapper)
        {
            _context= context;
            _mapper= mapper;
        }
        public async Task<List<ProjectDto>> GetAllAsync()
        {
            var projects = await _context.Projects
            .Include(x => x.ProjectMembers)
            .ThenInclude(pm => pm.User)
            .ToListAsync();
            if (!projects.Any())
            {
                throw new Exception("there are not any projects yes");
            }
            var projects_result = _mapper.Map<List<ProjectDto>>(projects);
            return projects_result;
        }

        public async Task<ProjectDto> GetByIdAsync(Guid id)
        {
            var project=await _context.Projects.Include(x=>x.ProjectMembers).ThenInclude(y=>y.User).FirstOrDefaultAsync(k=>k.Id==id);
            if(project==null)
            {
                throw new Exception("not found project by this Id");
            }
            var project_result = _mapper.Map<ProjectDto>(project);
            return project_result;
        }

        public async Task<List<ProjectDto>> GetByNameAsync(string name)
        {
            var project=await _context.Projects.Where(x=>x.Name==name).ToListAsync();
            if (project == null)
            {
                throw new Exception("there are not any project by this name");
            }
            var project_result = _mapper.Map<List<ProjectDto>>(project);
            return project_result;
        }

        public async Task<ProjectDto> CreateAsync(CreateProjectDto create_dto)
        {
            var project = _mapper.Map<Project>(create_dto);
            await _context.AddAsync(project);
            await _context.SaveChangesAsync();
            var project_result = _mapper.Map<ProjectDto>(project);
            return project_result;
        }

        public async Task<ProjectDto> UpdateAsync(UpdateProjectDto update_dto, Guid id)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(x => x.Id == id);

            if (project == null)
                throw new Exception("there is not any project by this id");

            _mapper.Map(update_dto, project); 

            await _context.SaveChangesAsync();

            return _mapper.Map<ProjectDto>(project);
        }

        public async Task DeleteAsync(Guid id)
        {
            var project= await _context.Projects.FirstOrDefaultAsync(x=>x.Id==id);
            if(project == null)
            {
                throw new Exception("there is not any project by this id");
            }
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

        }


        public async Task DeleteMemberFromProjectAsync(Guid project_id, Guid user_id)
        {
            var member = await _context.ProjectMembers.FirstOrDefaultAsync(x => x.ProjectId == project_id && x.UserId == user_id);
            if(member==null)
            {
                throw new Exception("not found prject or memebr  by these parametrs");
            }
            _context.ProjectMembers.Remove(member);
            await _context.SaveChangesAsync();
        }
        public async Task<List<UserDto>> GetProjectMembersAsync(Guid projectId)
        {
            var members = await _context.ProjectMembers
                .Where(pm => pm.ProjectId == projectId)
                .Include(pm => pm.User)
                .Select(pm => pm.User)
                .ToListAsync();

            return _mapper.Map<List<UserDto>>(members);
        }

        
        public async Task AddMemberToPrpjectAsync(Guid user_id, Guid project_id)
        {
            var member = new ProjectMember
            {
                ProjectId = project_id,
                UserId = user_id
            };
            await _context.ProjectMembers.AddAsync(member);  ///////// აქ უნდა მოვძებნო ობიექტი და დავმატო მემბერი
            await _context.SaveChangesAsync();
        }
    }
}
