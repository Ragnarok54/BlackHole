using BlackHole.Domain.DTO.User;
using BlackHole.Domain.Entities;
using BlackHole.Domain.Interfaces;
using System;
using System.Security.Cryptography;
using System.Text;

namespace BlackHole.Business.Services
{
    public class UserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public User Register(RegisterModel model)
        {
            User user = null;

            model.PhoneNumber = model.PhoneNumber.TrimEnd(' ');

            if (_unitOfWork.UserRepository.Get(model.PhoneNumber) == null)
            {
                var salt = GenerateSalt();

                user = new User
                {
                    PhoneNumber = model.PhoneNumber,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Salt = salt,
                    PasswordHash = CreatePasswordHash(model.Password, salt)
                };

                _unitOfWork.UserRepository.Add(user);
            }

            return user;
        }

        public User Login(string phoneNumber, string password)
        {
            var user = _unitOfWork.UserRepository.Get(phoneNumber);

            if (user.PasswordHash == CreatePasswordHash(password, user.Salt))
            {
                return user;
            }
            else
            {
                return null;
            }
        }

        private string GenerateSalt()
        {
            byte[] salt = new byte[50 / 8];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            return Convert.ToBase64String(salt);
        }

        private string CreatePasswordHash(string password, string salt)
        {
            using var sha1 = SHA1.Create();
            {
                var hashedBytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(password + salt));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }
}
