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


        public UserModel() { }

        public UserModel(Entities.User user)
        {
            UserId = user.UserId;
            FirstName = user.FirstName;
            LastName = user.LastName;
            PhoneNumber = user.PhoneNumber;
            Picture = user.Picture != null ? Convert.ToBase64String(user.Picture) : null;
        }
    }
}
