using CRM.Core.Business.Repositories;
using CRM.Core.Business.UseCases.MarkAsDeletedRange;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.Tests.UseCases.MarkAsDeletedRange
{
    [TestClass]
    public class MarkAsDeletedRangeHandlerTests
    {
        private readonly MarkAsDeletedRangeHandler _handle;
        private readonly Mock<IUserRepository> _repoMock = new();

        public MarkAsDeletedRangeHandlerTests()
        {
            _handle = new(_repoMock.Object);
            _repoMock.Setup(u => u.IsActivatedUserAsync(It.IsAny<string>())).ReturnsAsync(true);
        }

        [TestMethod]
        public async Task MarkAsDeletedRangeHandler_Handler_Return_False_If_Id_List_Empty()
        {
            // Arrange
            var query = new MarkAsDeletedRangeQuery();

            // Act
            var result = await _handle.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsFalse(result);
            _repoMock.Verify(r => r.MarkAsDeletedRangeAsync(It.IsAny<List<Guid>>(), It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task MarkAsDeletedRangeHandler_Return_True_If_Query_Is_Valid()
        {
            // Arrange
            var query = new MarkAsDeletedRangeQuery { Ids = new List<Guid> { Guid.NewGuid() }, UserName = "Test" };
            _repoMock
                .Setup(r => r.MarkAsDeletedRangeAsync(It.IsAny<List<Guid>>(), It.IsAny<string>()))
                .Returns(Task.FromResult(true));

            // Act
            var result = await _handle.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsTrue(result);
            _repoMock.Verify(r => r.MarkAsDeletedRangeAsync(It.IsAny<List<Guid>>(), It.IsAny<string>()), Times.Once);
        }
    }
}
