using CRM.Core.Business.Authentication;
using CRM.Core.Business.Models;
using CRM.Core.Business.Repositories;
using CRM.Core.Business.UseCases.Login;
using CRM.Core.Domain.Entities;
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
        }

        [TestMethod]
        public async Task LoginHandler_Throw_Unautorized_Exception()
        {
            // Arrange
            var loginQ = new LoginQuery
            {
                UserName = "Test",
                Password = "Test"
            };
            Tuple<User, List<Role>>? user= null;

            _userRepository
                .Setup(s => s.GetByUserAndRoleAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(user);
            _jwtService
                .Setup(j => j.Generate(It.IsAny<User>(), It.IsAny<List<Role>>()))
                .Returns("");

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
                Password = "Test"
            };
            Mock<User> userMock= new();
            Mock<List<Role>> rolesMock= new();
            Tuple<User, List<Role>>? userRole= new Tuple<User, List<Role>>(userMock.Object, rolesMock.Object);

            _userRepository
                .Setup(s => s.GetByUserAndRoleAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(userRole);
            _jwtService
                .Setup(j => j.Generate(It.IsAny<User>(), It.IsAny<List<Role>>()))
                .Returns("");

            // Act
            var result = await _handler.Handle(loginQ, CancellationToken.None);
            
            // Assert
            _userRepository.Verify(v => v.GetByUserAndRoleAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            _jwtService.Verify(j => j.Generate(It.IsAny<User>(), It.IsAny<List<Role>>()), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(UserModel));
        }
    }
}
