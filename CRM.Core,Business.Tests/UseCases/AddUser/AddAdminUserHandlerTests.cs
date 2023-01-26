using CRM.Core.Business.Models;
using CRM.Core.Business.Repositories;
using CRM.Core.Business.UseCases.AddUser;
using CRM.Core.Domain.Entities;
using CRM.Core.Domain.Exceptions;
using CRM.Core.Domain.Extensions;
using Moq;

namespace CRM.Core_Business.Tests.UseCases.AddUser;

[TestClass]
public class AddAdminUserHandlerTests
{
    private readonly Mock<IUserRepository> _repo = new();
    private readonly UserModel _um;

    public AddAdminUserHandlerTests()
    {
        _um = new UserModel { };
    }

    [TestMethod]
    public async Task Should_Return_success_creation()
    {
        // Arrange
        var handler = new AddAdminUserHandler(_repo.Object);
        var command = new AddAdminUserCommand()
        {
            Email = _um.Email,
            Password = "12345678",

        };
        var pwd = "test";
        var role = "test";
        var user = new User() { };
        _repo.Setup(r => r.AddAsync(user, pwd, role)).ReturnsAsync(_um);

        // Act
        var result = handler.Handle(command, CancellationToken.None).Result;

        // Assert
        _repo.Verify(d => d.AddAsync(user, pwd, role), Times.Once);
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task Should_Throw_Exception_Invalid_Email_Field()
    {
        // Arrange
        var handler = new AddAdminUserHandler(_repo.Object);
        var command = new AddAdminUserCommand()
        {
            Email= "test@test.com",
            Password = "12345678"
        };
        var pwd = "test";
        var role = "test";
        var user = new User() { };
        var expectedR = new Dictionary<string, List<string>>
        {
            {"Email", new List<string> { "email".ToInvalidMsg() } }
        };

        // Act
        var result = await Assert.ThrowsExceptionAsync<BaseException>(() => handler.Handle(command, CancellationToken.None));
        
        // Assert
        _repo.Verify(d => d.AddAsync(user, pwd, role), Times.Once);
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(BaseException));
        Assert.Equals(expectedR.Count, result.Errors.Count);
        CollectionAssert.AreEquivalent(expectedR["Email"], result.Errors["Email"]);
    }
}
