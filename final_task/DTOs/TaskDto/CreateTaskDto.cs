using System.ComponentModel.DataAnnotations;

namespace final_task.DTOs.TasksDto
{
    public class CreateTaskDto
    {

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Deadline is required.")]
        public DateTime Deadline { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [RegularExpression("^(Pending|InProgress|Done)$",
         ErrorMessage = "Status must be: Pending, InProgress or Done")]
        public string Status { get; set; } = null!;

        public Guid? UserId { get; set; }

        [Required(ErrorMessage = "ProjectId is required.")]
        public Guid ProjectId { get; set; }
    }
}

