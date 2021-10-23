namespace BlackHole.Domain.DTO.User
{
    public class RegisterModel : LoginModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
