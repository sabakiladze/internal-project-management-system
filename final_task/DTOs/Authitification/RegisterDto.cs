using System.ComponentModel.DataAnnotations;

namespace final_task.DTOs.Authitification
{
    public class RegisterDto
    {
        [Required(ErrorMessage ="Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [MaxLength(300, ErrorMessage ="max 300 symbols")]
        public string Email {  get; set; }

        [Required(ErrorMessage="this field is required")]
        [MinLength(2, ErrorMessage= "min 8 symbols")]
        [MaxLength(100, ErrorMessage = "max 600 symbols")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d).+$",
         ErrorMessage = "Password must contain at least one uppercase letter and one number")]
        public string Password { get; set; }

        [Required(ErrorMessage ="this field is required")]
        [MinLength(2, ErrorMessage ="min 2 symbols")]
        [MaxLength(100, ErrorMessage ="max 100 symbols")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "this field is required")]
        [MinLength(2, ErrorMessage = "min 2 symbols")]
        [MaxLength(100, ErrorMessage = "max 100 symbols")]
        public string LastName { get; set; }



    }

}
