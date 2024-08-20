using System.ComponentModel.DataAnnotations;

namespace ProductAPI.DTO
{
    public class RegisterModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        public string Role { get; set; } = "User";

    }
}
