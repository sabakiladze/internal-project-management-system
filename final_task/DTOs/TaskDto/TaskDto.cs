namespace final_task.DTOs.TaskDto
{
    public class TaskDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public DateTime Deadline { get; set; }

        public string Status { get; set; } = null!;

        public Guid? UserId { get; set; }

        public Guid ProjectId { get; set; }
    }
}
