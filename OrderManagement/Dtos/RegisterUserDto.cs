using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Api.Dtos
{
    public class RegisterUserDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [RegularExpression("(?=^.{6,10}$)(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[!@#$%^&amp;*()_+]).*$"
            , ErrorMessage = "Password must contain 1 uppercase,1 lowercase ,1 Digget ,1 Special Character")]
        public string Password { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
    }
}
