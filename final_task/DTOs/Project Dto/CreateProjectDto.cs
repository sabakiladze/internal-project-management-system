using System.ComponentModel.DataAnnotations;

namespace final_task.DTOs
{
    public class CreateProjectDto
    {
        [Required(ErrorMessage ="this field is required")]
        [MinLength(2, ErrorMessage ="min length of project's the name is 2 characters")]
        [MaxLength(200, ErrorMessage ="max length of the project's name is 200 charachters")]
        public string Name { get; set; } = null!;

        [MinLength(10, ErrorMessage = "min is 10 symbols")]
        [MaxLength(200, ErrorMessage = "max is 200 symbols")]

        public string? Description { get; set; }

        [Required(ErrorMessage = "this field is required.")]
        public DateTime Deadline { get; set; }
    }
}
