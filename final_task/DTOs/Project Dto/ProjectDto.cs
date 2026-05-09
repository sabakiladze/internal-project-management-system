using final_task.DTOs.Authitification;

namespace final_task.DTOs
{
    public class ProjectDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? Deadline { get; set; }
        public List<UserDto> Members { get; set; }
    }
}
