using CRM.Core.Business.Authentication;
using CRM.Core.Business.Models;
using CRM.Core.Business.Repositories;
using CRM.Core.Business.UseCases.Login;
using CRM.Core.Domain.Entities;
using CRM.Core.Domain.Exceptions;
using Moq;

namespace CRM.Core.Business.Tests.UseCases.Login
{
    [TestClass]
    public class LoginHandlerTests
    {
        private readonly LoginHandler _handler;
        private readonly Mock<IUserRepository> _userRepository = new();
        private readonly Mock<IJWTService> _jwtService = new();

        public LoginHandlerTests()
        {
            _handler= new LoginHandler(_userRepository.Object, _jwtService.Object);
            _userRepository.Setup(u => u.IsActivatedUserAsync(It.IsAny<string>())).ReturnsAsync(true);
        }

        [TestMethod]
        public async Task LoginHandler_Throw_BaseException_UserName_Or_Password_Invalid()
        {
            // Arrange
            var loginQ = new LoginQuery
            {
                UserName = "",
                Password = ""
            };
            Tuple<User, List<Role>>? user= null;

            _userRepository
                .Setup(s => s.GetByUserAndRoleAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(user);
            _jwtService
                .Setup(j => j.Generate(It.IsAny<User>(), It.IsAny<List<Role>>()))
                .Returns("");

            // Act
            var result = await Assert.ThrowsExceptionAsync<BaseException>(() => _handler.Handle(loginQ, CancellationToken.None));
            
            // Assert
            _userRepository.Verify(v => v.GetByUserAndRoleAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _jwtService.Verify(j => j.Generate(It.IsAny<User>(), It.IsAny<List<Role>>()), Times.Never);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BaseException));
        }

        [TestMethod]
        public async Task LoginHandler_Throw_Unautorized_Exception()
        {
            // Arrange
            var loginQ = new LoginQuery
            {
                UserName = "Test",
                Password = "12345678"
            };
            Tuple<User, List<Role>>? user= null;

            _userRepository
                .Setup(s => s.GetByUserAndRoleAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(user);
            _jwtService
                .Setup(j => j.Generate(It.IsAny<User>(), It.IsAny<List<Role>>()))
                .Returns("1234");

            // Act
            var result = await Assert.ThrowsExceptionAsync<UnauthorizedAccessException>(() => _handler.Handle(loginQ, CancellationToken.None));
            
            // Assert
            _userRepository.Verify(v => v.GetByUserAndRoleAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            _jwtService.Verify(j => j.Generate(It.IsAny<User>(), It.IsAny<List<Role>>()), Times.Never);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task LoginHandler_Give_User_And_Token_Where_User_Found()
        {
            // Arrange
            var loginQ = new LoginQuery
            {
                UserName = "Test",
                Password = "12345678"
            };
            Mock<User> userMock= new();
            Mock<List<Role>> rolesMock= new();
            Tuple<User, List<Role>>? userRole= new Tuple<User, List<Role>>(userMock.Object, rolesMock.Object);

            _userRepository
                .Setup(s => s.GetByUserAndRoleAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(userRole);
            _jwtService
                .Setup(j => j.Generate(It.IsAny<User>(), It.IsAny<List<Role>>()))
                .Returns("1234");

            // Act
            var result = await _handler.Handle(loginQ, CancellationToken.None);
            
            // Assert
            _userRepository.Verify(v => v.GetByUserAndRoleAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            _jwtService.Verify(j => j.Generate(It.IsAny<User>(), It.IsAny<List<Role>>()), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(UserModel));
            Assert.AreEqual(result.Token, "1234");
        }
    }
}
