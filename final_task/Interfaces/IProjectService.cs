using final_task.DTOs;
using final_task.DTOs.Authitification;

namespace final_task.Interfaces
{
    public interface IProjectService
    {
        Task<List<ProjectDto>> GetAllAsync();
        Task<ProjectDto> GetByIdAsync(Guid id);
        Task<List<ProjectDto>> GetByNameAsync(string name);
        Task<ProjectDto> CreateAsync(CreateProjectDto create_dto);
        Task<ProjectDto> UpdateAsync(UpdateProjectDto dto, Guid id);
        Task DeleteAsync(Guid id);
        Task AddMemberToPrpjectAsync(Guid user_id, Guid project_id);
        Task DeleteMemberFromProjectAsync(Guid project_id, Guid user_id);
        Task<List<UserDto>> GetProjectMembersAsync(Guid projectId);
        
    }
}
