using BlackHole.Business.Services;
using BlackHole.Domain.DTO.User;
using BlackHole.Domain.Interfaces;
using BlackHole.UnitTests.TestRepositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlackHole.UnitTests.ServiceTests
{
    [TestClass]
    public class UserServiceTest
    {
        private IUnitOfWork _unitOfWork;
        private UserService _userService;

        [TestInitialize]
        public void InitializeData()
        {
            _unitOfWork = new TestUnitOfWork();
            _userService = new UserService(_unitOfWork);
        }

        [TestMethod]
        public void UserLogin_ShouldWorkForNewUser_WithCorrectPassword()
        {
            var userRegisterModel = new RegisterModel
            {
                FirstName = "Register test",
                LastName = "Last name",
                PhoneNumber = "test@zap.com",
                Password = "HashTest*0"
            };

            _userService.Register(userRegisterModel);

            var result = _userService.Login(userRegisterModel.PhoneNumber, userRegisterModel.Password);

            Assert.IsTrue(result != null);
        }

        [TestMethod]
        public void UserLogin_ShouldFailForNewUser_WithWrongPassword()
        {
            var userRegisterModel = new RegisterModel
            {
                FirstName = "Register test",
                LastName = "Last name",
                PhoneNumber = "test@zap.com",
                Password = "HashTest*0"
            };

            _userService.Register(userRegisterModel);

            var result = _userService.Login(userRegisterModel.PhoneNumber, "Wrong Password");

            Assert.IsFalse(result != null);
        }

    }
}
