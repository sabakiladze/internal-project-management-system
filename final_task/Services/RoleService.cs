using AutoMapper;
using final_task.DTOs;
using final_task.DTOs.Authitification;
using final_task.Interfaces;
using final_task.Models;
using Microsoft.EntityFrameworkCore;

namespace final_task.Services
{
    public class RoleService : IRoleService
    {
        private readonly IMapper _mapper;
        private readonly UsersContext _roleContext;
        public RoleService(IMapper mapper, UsersContext rolecontext)
        {
            _mapper = mapper;
            _roleContext = rolecontext;
        }

        public Task<UserDto> AssigneRoleToUser(Guid user_id, Guid role_id)
        {
            throw new NotImplementedException();
        }

        public async Task<UserDto> AssignRoleToUser(Guid user_id, Guid role_id)
        {
            var user = await _roleContext.Users
                .FirstOrDefaultAsync(x => x.Id == user_id);

            if (user == null)
                throw new Exception("user not found");

            var role = await _roleContext.Roles
                .FirstOrDefaultAsync(x => x.Id == role_id);

            if (role == null)
                throw new Exception("role not found");

            var alreadyExists = await _roleContext.UsersRoles
                .AnyAsync(x => x.UsersId == user_id && x.RoleId == role_id);

            if (alreadyExists)
                throw new Exception("user already has this role");

            var userRole = new UsersRole
            {
                UsersId = user_id,
                RoleId = role_id,
                AssignedAr = DateTime.UtcNow
            };

            await _roleContext.UsersRoles.AddAsync(userRole);
            await _roleContext.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }
        public async Task<RoleDto> CreateRoleAsync(CreateRoleDto dto)
        {
            var role = _mapper.Map<Role>(dto);
            await _roleContext.Roles.AddAsync(role);
            await _roleContext.SaveChangesAsync();
            var role_return = _mapper.Map<RoleDto>(role);
            return role_return;
        }

        public async Task DeleteRoleAsync(Guid id)
        {
            var role=await _roleContext.Roles.FirstOrDefaultAsync(x=>x.Id==id);
            if(role==null)
            {
                throw new Exception("not found your role");
            }
            _roleContext.Roles.Remove(role);
            await _roleContext.SaveChangesAsync();
        }

        public async Task<List<RoleDto>> GetAllRoleAsync()
        {
            var all = await _roleContext.Roles.ToListAsync();
            if(!all.Any())
            {
                throw new Exception("there are not any roles yet");
            }
            var alls = _mapper.Map<List<RoleDto>>(all);
            return alls;
        }

        public async Task<RoleDto> GetByIdRoleAsync(Guid id)
        {
            var role=await _roleContext.Roles.FirstOrDefaultAsync(x=>x.Id==id);
            if(role==null)
            {
                throw new Exception("role not found");
            }
            var role_return = _mapper.Map<RoleDto>(role);
            return role_return;
        }

        public  async Task<RoleDto> UpdateRoleAsync(Guid id, UpdateRoleDto dto)
        {
            var role=await _roleContext.Roles.FirstOrDefaultAsync(x=>x.Id==id);
            if(role==null)
            {
                throw new Exception("role not found");
            }
            _mapper.Map(dto, role);
            await _roleContext.SaveChangesAsync();
            var role_return=_mapper.Map<RoleDto>(role);
            return role_return;
        }
        
        
    }
}
