using AutoMapper;
using final_task.DTOs.TaskDto;
using final_task.DTOs.TasksDto;
using final_task.Interfaces;
using final_task.Models;
using Microsoft.EntityFrameworkCore;

namespace final_task.Services
{
    public class TasksProjectService : ITasksProject
    {
        private readonly IMapper _mapper;
        private readonly UsersContext _TaskContext;

        public TasksProjectService(IMapper mapper, UsersContext context)
        {
            _mapper = mapper;
            _TaskContext = context;
        }

        public async Task<TaskDto> CreateTask(CreateTaskDto dto)
        {
            var task = _mapper.Map<TasksProject>(dto);

            await _TaskContext.Tasks.AddAsync(task);
            await _TaskContext.SaveChangesAsync();

            var task_return =_mapper.Map<TaskDto>(task);
            return task_return;
        }


        public async Task<List<TaskDto>> GetAllTask()
        {
            var tasks = await _TaskContext.Tasks.ToListAsync();
            return _mapper.Map<List<TaskDto>>(tasks);
        }

        public async Task<TaskDto> GetTaskById(Guid id)
        {
            var task = await _TaskContext.Tasks.FirstOrDefaultAsync(x => x.Id == id);

            if (task == null)
                throw new Exception("Task not found");

            return _mapper.Map<TaskDto>(task);
        }

        public async Task<List<TaskDto>> GetTaskByName(string name)
        {
            var tasks = await _TaskContext.Tasks
                .Where(x => x.Name == name)
                .ToListAsync();

            if (!tasks.Any())
                throw new Exception("No tasks found with this name");

            return _mapper.Map<List<TaskDto>>(tasks);
        }

        public async Task<List<TaskDto>> GetTasksByProjectIdAsync(Guid projectId)
        {
            var tasks = await _TaskContext.Tasks
                .Where(x => x.ProjectId == projectId)
                .ToListAsync();

            return _mapper.Map<List<TaskDto>>(tasks);
        }

        public async Task<List<TaskDto>> GetTasksByUserIdAsync(Guid userId)
        {
            var tasks = await _TaskContext.Tasks
                .Where(x => x.UserId == userId)
                .ToListAsync();

            if (!tasks.Any())
                throw new Exception("This user has no tasks");

            return _mapper.Map<List<TaskDto>>(tasks);
        }

        public async Task AssignUserToTaskAsync(Guid taskId, Guid userId)
        {
            var task = await _TaskContext.Tasks.FirstOrDefaultAsync(x => x.Id == taskId);

            if (task == null)
                throw new Exception("Task not found");

            var userExists = await _TaskContext.Users.AnyAsync(x => x.Id == userId);

            if (!userExists)
                throw new Exception("User not found");

            task.UserId = userId;

            await _TaskContext.SaveChangesAsync();
        }

        public async Task RemoveUserFromTaskAsync(Guid taskId)
        {
            var task = await _TaskContext.Tasks.FirstOrDefaultAsync(x => x.Id == taskId);

            if (task == null)
                throw new Exception("Task not found");

            task.UserId = null;

            await _TaskContext.SaveChangesAsync();
        }

        public async Task ChangeTaskStatusAsync(Guid taskId, string status)
        {
            var task = await _TaskContext.Tasks.FirstOrDefaultAsync(x => x.Id == taskId);

            if (task == null)
                throw new Exception("Task not found");

            string[] statuses = { "Pending", "InProgress", "Done" };

            if (!statuses.Contains(status))
                throw new Exception("Invalid status");

            task.Status = status;

            await _TaskContext.SaveChangesAsync();
        }

        public async Task DeleteTask(Guid id)
        {
            var task = await _TaskContext.Tasks.FirstOrDefaultAsync(x => x.Id == id);

            if (task == null)
                throw new Exception("Task not found");

            _TaskContext.Tasks.Remove(task);

            await _TaskContext.SaveChangesAsync();

        }

        public async Task<TaskDto> UpdateTask(Guid id, UpdateTaskDto dto)
        {
            var task = await _TaskContext.Tasks.FirstOrDefaultAsync(x => x.Id == id);

            if (task == null)
                throw new Exception("Task not found");

            _mapper.Map(dto, task);

            await _TaskContext.SaveChangesAsync();

            return _mapper.Map<TaskDto>(task);
        }

        
    }
}