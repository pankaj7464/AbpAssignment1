using System.ComponentModel.DataAnnotations;

namespace EMS.DTOs
{
    public class CreateUserDto
    {
        [Required]
        public string username { get; set; }
        [Required]
        public string Password { get; set; }

        [Required,EmailAddress]

        public string Email { get; set; }
    }
}