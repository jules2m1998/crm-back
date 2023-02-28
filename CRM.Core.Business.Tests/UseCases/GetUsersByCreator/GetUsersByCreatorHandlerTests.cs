using CRM.Core.Business.Models;
using CRM.Core.Business.Repositories;
using CRM.Core.Business.UseCases.GetUsersByCreator;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.Tests.UseCases.GetUsersByCreator
{
    [TestClass]
    public class GetUsersByCreatorHandlerTests
    {
        private GetUsersByCreatorHandler _handler;
        private readonly Mock<IUserRepository> _userRepositoryMock = new();

        public GetUsersByCreatorHandlerTests()
        {
            _handler = new(_userRepositoryMock.Object);
            _userRepositoryMock.Setup(u => u.IsActivatedUserAsync(It.IsAny<string>())).ReturnsAsync(true);
        }

        [TestMethod]
        public async Task GetUsersByCreatorHandler_Handler_Throw_Invalid_Argurment_When_Username_Is_not_define()
        {
            // Arrange
            var cmd = new GetUsersByCreatorQuery();

            // Act
            var result = await Assert.ThrowsExceptionAsync<UnauthorizedAccessException>(() => _handler.Handle(cmd, CancellationToken.None));

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(UnauthorizedAccessException));
            _userRepositoryMock.Verify(u => u.GetUsersByCreatorUserNameAsync(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task GetUsersByCreatorHandler_Handler_Return_List_Of_User_When_all_Right()
        {
            // Arrange
            var rlist = new List<UserAndCreatorModel>()
            {
                new(Guid.NewGuid(), "Test", "test", "test", "test", new List<string>{"test"}, null, null, DateTime.UtcNow, null, null, new())
            };
            var cmd = new GetUsersByCreatorQuery { CreatorUserName = "test" };
            _userRepositoryMock
                .Setup(u => u.GetUsersByCreatorUserNameAsync(cmd.CreatorUserName))
                .ReturnsAsync(rlist);

            // Act
            var result = await _handler.Handle(cmd, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ICollection<UserAndCreatorModel>));
            CollectionAssert.AreEquivalent(rlist, result.ToList());
            _userRepositoryMock.Verify(u => u.GetUsersByCreatorUserNameAsync(cmd.CreatorUserName), Times.Once);
        }
    }
}
