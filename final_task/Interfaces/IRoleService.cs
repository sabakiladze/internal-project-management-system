using final_task.DTOs;
using final_task.DTOs.Authitification;

namespace final_task.Interfaces
{
    public interface IRoleService
    {
        Task<RoleDto> CreateRoleAsync(CreateRoleDto dto);
        Task DeleteRoleAsync(Guid id);
        Task<RoleDto>UpdateRoleAsync(Guid id, UpdateRoleDto dto);
        Task<RoleDto>GetByIdRoleAsync(Guid id);
        Task<List<RoleDto>> GetAllRoleAsync();
        Task<UserDto> AssignRoleToUser(Guid user_id, Guid role_id);
    }
}
