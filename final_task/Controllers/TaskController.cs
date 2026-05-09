using final_task.DTOs.TaskDto;
using final_task.DTOs.TasksDto;
using final_task.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace final_task.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ITasksProject _tasksService;

        public TasksController(ITasksProject tasksService)
        {
            _tasksService = tasksService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult> CreateTask(CreateTaskDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

           var result= await _tasksService.CreateTask(dto);
            return Ok(result);
        }
        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<List<TaskDto>>> GetAllTasks()
        {
            var tasks = await _tasksService.GetAllTask();
            return Ok(tasks);
        }


        



        [HttpGet("{id}")]
        public async Task<ActionResult<TaskDto>> GetTaskById(Guid id)
        {
            try
            {
                var task = await _tasksService.GetTaskById(id);
                return Ok(task);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("by-name/{name}")]
        public async Task<ActionResult<List<TaskDto>>> GetTaskByName(string name)
        {
            try
            {
                var tasks = await _tasksService.GetTaskByName(name);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("by-project/{projectId}")]
        public async Task<ActionResult<List<TaskDto>>> GetTasksByProjectId(Guid projectId)
        {
            var tasks = await _tasksService.GetTasksByProjectIdAsync(projectId);
            return Ok(tasks);
        }

        [HttpGet("by-user/{userId}")]
        public async Task<ActionResult<List<TaskDto>>> GetTasksByUserId(Guid userId)
        {
            try
            {
                var tasks = await _tasksService.GetTasksByUserIdAsync(userId);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TaskDto>> UpdateTask(Guid id, [FromBody] UpdateTaskDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var task = await _tasksService.UpdateTask(id, dto);
                return Ok(task);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPut("{taskId}/assign/{userId}")]
        public async Task<IActionResult> AssignUserToTask(Guid taskId, Guid userId)
        {
            try
            {
                await _tasksService.AssignUserToTaskAsync(taskId, userId);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{taskId}/remove-user")]
        public async Task<IActionResult> RemoveUserFromTask(Guid taskId)
        {
            try
            {
                await _tasksService.RemoveUserFromTaskAsync(taskId);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPut("{taskId}/status")]
        public async Task<IActionResult> ChangeTaskStatus(Guid taskId, [FromQuery] string status)
        {
            try
            {
                await _tasksService.ChangeTaskStatusAsync(taskId, status);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            try
            {
                await _tasksService.DeleteTask(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
