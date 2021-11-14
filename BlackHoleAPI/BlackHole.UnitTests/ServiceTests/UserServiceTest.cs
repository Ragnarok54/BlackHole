using BlackHole.Business.Services;
using BlackHole.Domain.DTO.User;
using BlackHole.Domain.Interfaces;
using BlackHole.UnitTests.TestRepositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        public void UserLogin_ShouldNotDependOn_PhoneNumberWhitespace()
        {
            var userRegisterModel = new RegisterModel
            {
                FirstName = "Login test",
                LastName = "Login test",
                PhoneNumber = "0799999999",
                Password = "Password"
            };
            
            _userService.Register(userRegisterModel);

            var resultWithWhitespaces = _userService.Login("0799 999 999", "Password");
            var resultWithoutWhitespaces = _userService.Login("0799999999", "Password");
            var isLoggedInWithWhitespaces = resultWithWhitespaces != null;
            var isLoggedInWithoutWhitespaces = resultWithoutWhitespaces != null;

            Assert.IsTrue(isLoggedInWithWhitespaces);
            Assert.IsTrue(isLoggedInWithoutWhitespaces);
        }

        [TestMethod]
        public void UserLogin_ShouldWorkForNewUser_WithCorrectPassword()
        {
            var userRegisterModel = new RegisterModel
            {
                FirstName = "Register test",
                LastName = "Last name",
                PhoneNumber = "081231412",
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
                PhoneNumber = "081231412",
                Password = "Password"
            };

            _userService.Register(userRegisterModel);

            var result = _userService.Login(userRegisterModel.PhoneNumber, "Wrong Password");

            Assert.IsFalse(result != null);
        }

        [TestMethod]
        public void UserLogin_ShouldFail_ForNonexistentUser()
        {
            var result = _userService.Login("phoneNumber", "password");
            var isLoggedIn = result != null;

            Assert.IsFalse(isLoggedIn);
        }

        [TestMethod]
        public void UserRegister_ShouldFail_WhenUsingExistingPhoneNumber()
        {
            var userRegisterModel1 = new RegisterModel
            {
                FirstName = "Register test",
                LastName = "Last name",
                PhoneNumber = "1412312",
                Password = "HashTest*0"
            };

            _userService.Register(userRegisterModel1);

            var userRegisterModel2 = new RegisterModel
            {
                FirstName = "Register test 2",
                LastName = "Last name 2",
                PhoneNumber = "1412312",
                Password = "HashTest*0"
            };

            var result = _userService.Register(userRegisterModel2);
            var isRegistered = result != null;

            Assert.IsFalse(isRegistered);
        }

    }
}
