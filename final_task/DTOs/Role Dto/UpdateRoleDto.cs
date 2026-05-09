using System.ComponentModel.DataAnnotations;

namespace final_task.DTOs
{
    public class UpdateRoleDto
    {
        [Required(ErrorMessage = "this field is required")]
        [MinLength(2, ErrorMessage = "min is 2 symbols")]
        [MaxLength(100, ErrorMessage = "max is 100 symbols")]
        public string Name { get; set; }


        [MinLength(10, ErrorMessage = "min is 10 symbols")]
        [MaxLength(200, ErrorMessage = "max is 200 symbols")]
        public string? Description { get; set; }

    }
}
