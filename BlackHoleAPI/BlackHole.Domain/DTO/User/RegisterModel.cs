using System.ComponentModel.DataAnnotations;

namespace BlackHole.Domain.DTO.User
{
    public class RegisterModel : LoginModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Picture { get; set; }
    }
}
