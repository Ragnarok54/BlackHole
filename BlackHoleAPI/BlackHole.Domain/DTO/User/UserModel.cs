using System;
using BlackHole.Domain.Entities;

namespace BlackHole.Domain.DTO.User
{
    public class UserModel
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name => FirstName + " " + LastName;
        public string PhoneNumber { get; set; }
        public string Picture { get; set; }
    }
}
