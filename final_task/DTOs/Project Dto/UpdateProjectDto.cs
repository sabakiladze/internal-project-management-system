using System.ComponentModel.DataAnnotations;

namespace final_task.DTOs
{
    public class UpdateProjectDto
    {
        [Required(ErrorMessage = "this field is required")]
        [MinLength(2, ErrorMessage = "min length of project's the name is 2 characters")]
        [MaxLength(200, ErrorMessage = "max length of the project's name is 200 charachters")]
        public string Name { get; set; } = null!;

        [MinLength(10, ErrorMessage = "min is 10 symbols")]
        [MaxLength(500, ErrorMessage = "max is 500 symbols")]

        public string? Description { get; set; }

        [Required(ErrorMessage = "this field is required.")]
        public DateTime Deadline { get; set; }
    }
}
