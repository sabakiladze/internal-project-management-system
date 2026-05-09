using final_task.DTOs;
using final_task.Interfaces;
using final_task.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace final_task.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService roleService;
        private readonly UsersContext usersContext;

        public RoleController(IRoleService Service)
        {
            roleService = Service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await roleService.GetAllRoleAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleId(Guid id)
        {
            try
            {
                var result = await roleService.GetByIdRoleAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddRole(CreateRoleDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await roleService.CreateRoleAsync(dto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(Guid id, UpdateRoleDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest("not provides necessary requirements");

            try
            {
                var result = await roleService.UpdateRoleAsync(id, dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRole(Guid id)
        {
            try
            {
                await roleService.DeleteRoleAsync(id);
                return Ok("deleted successfully");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpPost("addroletouser/{userId}/{roleId}")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> assigne(Guid userId, Guid roleId)
        {
            try
            {
                var result = await roleService.AssignRoleToUser(userId, roleId);
                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}