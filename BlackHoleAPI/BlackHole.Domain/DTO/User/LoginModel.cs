using System.ComponentModel.DataAnnotations;

namespace BlackHole.Domain.DTO.User
{
    public class LoginModel
    {
        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
