using final_task.Models;
using System.ComponentModel.DataAnnotations;

namespace final_task.DTOs.Authitification
{
    public class LogInDto
    {
       
            [Required(ErrorMessage = "Email is required")]
            [EmailAddress(ErrorMessage = "Invalid email format")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Password is required")]
            public string Password { get; set; }
       
    }
}


  