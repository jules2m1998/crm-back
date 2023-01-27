using CRM.Core.Business.Repositories;
using CRM.Core.Business.UseCases.GetOneUserByUsername;
using CRM.Core.Domain.Entities;
using Moq;

namespace CRM.Core.Business.Tests.UseCases.GetOneUserByUsername
{
    [TestClass]
    public class GetOneUserByUsernameHandlerTest
    {
        private readonly Mock<IUserRepository> _userRepository = new();
        private readonly GetOneUserByUsernameHandler _handler;

        public GetOneUserByUsernameHandlerTest()
        {
            _handler = new(_userRepository.Object);
        }

        [TestMethod]
        public async Task GetOneUserByUsernameHandler_Return_Null_If_Not_Exist()
        {
            Tuple<User, List<Role>>? mock = null;
            var query = new GetOneUserByUsernameQuery("tests");
            // Arrange
            _userRepository.Setup(v => v.GetUserAndRole(It.IsAny<string>()))
                .ReturnsAsync(mock);
            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            _userRepository.Verify(v => v.GetUserAndRole(It.IsAny<string>()), Times.Once);
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetOneUserByUsernameHandler_Return_UserAndRoleIfAlright()
        {
            Tuple<User, List<Role>>? mock = new Tuple<User, List<Role>>(new User { UserName = "Test" }, new List<Role>() { new Role { Name = "Test"} });
            var query = new GetOneUserByUsernameQuery("tests");
            // Arrange
            _userRepository.Setup(v => v.GetUserAndRole(It.IsAny<string>()))
                .ReturnsAsync(mock);
            _userRepository
                .Setup(u => u.UserToUserModel(It.IsAny<User>(), It.IsAny<List<Role>>()))
                .Returns(new Models.UserModel { UserName = "Test", Roles = new List<string> { "Test" } });

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            _userRepository.Verify(v => v.GetUserAndRole(It.IsAny<string>()), Times.Once);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.UserName, "Test");
            Assert.AreEqual(1, result.Roles.Count);
            CollectionAssert.AreEquivalent(mock.Item2.Select(r => r.Name).ToList(), result.Roles);
        }
    }
}
