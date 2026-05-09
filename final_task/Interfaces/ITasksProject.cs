using final_task.DTOs.TaskDto;
using final_task.DTOs.TasksDto;

namespace final_task.Interfaces
{
    public interface ITasksProject
    {
        Task<TaskDto> CreateTask(CreateTaskDto dto);
        Task<TaskDto> UpdateTask(Guid Iid, UpdateTaskDto dto);
        Task DeleteTask(Guid id);
        Task<TaskDto> GetTaskById(Guid id);
        Task<List<TaskDto>> GetTaskByName(string name);
        Task<List<TaskDto>> GetAllTask();
        Task AssignUserToTaskAsync(Guid taskId, Guid userId);
        Task RemoveUserFromTaskAsync(Guid taskId);
        Task<List<TaskDto>> GetTasksByProjectIdAsync(Guid projectId);
        Task ChangeTaskStatusAsync(Guid taskId, string status);
        Task<List<TaskDto>> GetTasksByUserIdAsync(Guid userId);
    }
}
