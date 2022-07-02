using BlackHole.Domain.DTO.User;
using BlackHole.Domain.Entities;
using BlackHole.Domain.Interfaces;
using System;
using System.Security.Cryptography;
using System.Text;

namespace BlackHole.Business.Services
{
    public class UserService : BaseService
    {
        public UserService(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public User GetUser(Guid userId)
        {
            return UnitOfWork.UserRepository.Get(userId);
        }

        public User Register(RegisterModel model)
        {
            User user = null;

            model.PhoneNumber = model.PhoneNumber.Replace(" ", string.Empty);

            if (UnitOfWork.UserRepository.Get(model.PhoneNumber) == null)
            {
                var salt = GenerateSalt();

                user = new User
                {
                    PhoneNumber = model.PhoneNumber,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Picture = model.Picture == null ? null : Convert.FromBase64String(model.Picture),
                    Salt = salt,
                    PasswordHash = CreatePasswordHash(model.Password, salt)
                };

                UnitOfWork.UserRepository.Add(user);
            }

            Save();

            return user;
        }

        public User Login(string phoneNumber, string password)
        {
            var user = UnitOfWork.UserRepository.Get(phoneNumber.Replace(" ", string.Empty));

            if (user?.PasswordHash == CreatePasswordHash(password, user?.Salt))
            {
                return user;
            }
            else
            {
                return null;
            }
        }

        public void Edit(UserModel model, Guid currentUserId)
        {
            var user = UnitOfWork.UserRepository.Get(currentUserId);

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;

            if (!string.IsNullOrEmpty(model.Picture))
            {
                user.Picture = Convert.FromBase64String(model.Picture);
            }

            UnitOfWork.SaveChanges();
        }

        public string GetPicture(Guid userId)
        {
            var user = UnitOfWork.UserRepository.Get(userId);

            return user.Picture == null ? null : Convert.ToBase64String(user.Picture);
        }

        private static string GenerateSalt()
        {
            byte[] salt = new byte[50 / 8];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            return Convert.ToBase64String(salt);
        }

        private static string CreatePasswordHash(string password, string salt)
        {
            using var sha1 = SHA1.Create();
            {
                var hashedBytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(password + salt));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }
}
