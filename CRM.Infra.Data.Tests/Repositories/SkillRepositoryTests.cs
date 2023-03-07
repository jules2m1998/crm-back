using CRM.Core.Business.Repositories;
using CRM.Core.Domain.Entities;
using CRM.Core.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace CRM.Infra.Data.Repositories.Tests
{
    [TestClass]
    public class SkillRepositoryTests
    {
        private readonly Mock<IApplicationDbContext> _dbContextMock = new();
        private readonly Mock<DbSet<Skill>> _setMock = new();
        private readonly SkillRepository _repo;
        private readonly Mock<IUserRepository> _userRepositoryMock = new();

        public SkillRepositoryTests()
        {
            _dbContextMock
                .Setup(db => db.Skills)
                .Returns(_setMock.Object);
            _setMock
                .Setup(s => s.AddRangeAsync(It.IsAny<IEnumerable<Skill>>(), CancellationToken.None))
                .Returns(Task.CompletedTask);
            _repo = new(_dbContextMock.Object);
        }

        [TestMethod]
        public async Task AddRangeAsyncTest_ThrowBaseException_When_Some_Skill_Invalid()
        {
            // Arrange
            var list = new List<Skill>
            {
                new Skill()
            };

            // Act
            var result = await Assert
                .ThrowsExceptionAsync<BaseException>(() => _repo.AddRangeAsync(list));

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BaseException));
            _setMock.Verify(v => v.AddRange(It.IsAny<List<Skill>>()), Times.Never);
        }

        [TestMethod]
        public async Task AddRangeAsyncTest_Return_List_Of_Skill_When_AllRight()
        {
            // Arrange
            var list = new List<Skill>
            {
                new Skill(
                    "Test", 
                    "Place", 
                    DateTime.Now, 
                    DateTime.Now.AddDays(1), 
                    false, 
                    new User(), 
                    null
                    )
            };

            // Act
            var result = await _repo.AddRangeAsync(list);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<Skill>));
            CollectionAssert.AreEquivalent(result.ToList(), list);
        }
    }
}