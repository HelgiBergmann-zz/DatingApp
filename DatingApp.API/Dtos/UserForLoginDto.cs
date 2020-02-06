using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Dtos
{
    public class UserForLoginDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [StringLength(200, MinimumLength = 6, ErrorMessage = "Password must include minimum 6 characters")]
        public string Password { get; set; }
    }
}