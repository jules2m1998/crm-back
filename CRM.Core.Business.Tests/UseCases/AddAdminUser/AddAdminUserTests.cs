using CRM.Core.Business.Helpers;
using CRM.Core.Business.Models;
using CRM.Core.Business.Repositories;
using CRM.Core.Business.UseCases.AddUser;
using CRM.Core.Domain.Entities;
using CRM.Core.Domain.Exceptions;
using CRM.Core.Domain.Extensions;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Threading.Tasks;

namespace CRM.Core.Business.Tests.UseCases.AddAdminUser
{
    [TestClass]
    public class AddAdminUserTests
    {
        private readonly AddAdminUserHandler _handler;
        private readonly Mock<IUserRepository> _repo = new();
        private readonly Mock<IFileHelper> _fileHelper = new();
        private readonly Mock<IFormFile> _file = new();

        public AddAdminUserTests()
        {
            _handler = new(_repo.Object, _fileHelper.Object);
        }

        [TestMethod]
        public async Task Should_Emit_BaseException_When_Command_Empty_Fields_Required()
        {
            // Arrange
            var command = new AddAdminUserCommand()
            {
            };


            // Act
            var result = await Assert.ThrowsExceptionAsync<BaseException>(() => _handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BaseException));
            Assert.AreEqual(5, result.Errors.Count);
            CollectionAssert.AreEquivalent(new List<string> { "UserName".ToRequiredMsg() }, result.Errors["UserName"]);
            CollectionAssert.AreEquivalent(new List<string> { "Email".ToRequiredMsg() }, result.Errors["Email"]);
            CollectionAssert.AreEquivalent(new List<string> { "Password".ToRequiredMsg() }, result.Errors["Password"]);
            CollectionAssert.AreEquivalent(new List<string> { "FirstName".ToRequiredMsg() }, result.Errors["FirstName"]);
            CollectionAssert.AreEquivalent(new List<string> { "LastName".ToRequiredMsg() }, result.Errors["LastName"]);
        }

        [TestMethod]
        public async Task Should_Emit_BaseException_When_Command_Pwd_Is_not_Valid()
        {
            // Arrange
            var command = new AddAdminUserCommand()
            {
                UserName = "admin",
                FirstName= "admin",
                LastName= "admin",
                Email = "mevaajules9@gmail.com",
                Password = "a"
            };


            // Act
            var result = await Assert.ThrowsExceptionAsync<BaseException>(() => _handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BaseException));
            Assert.AreEqual(1, result.Errors.Count);
            CollectionAssert.AreEquivalent(new List<string> { "The field Password must be a string or array type with a minimum length of '8'." }, result.Errors["Password"]);
        }

        [TestMethod]
        public async Task Should_Emit_BaseException_When_Command_Email_Is_not_Valid()
        {
            // Arrange
            var command = new AddAdminUserCommand()
            {
                UserName = "admin",
                FirstName= "admin",
                LastName= "admin",
                Email = "m",
                Password = "12345678"
            };


            // Act
            var result = await Assert.ThrowsExceptionAsync<BaseException>(() => _handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BaseException));
            Assert.AreEqual(1, result.Errors.Count);
            CollectionAssert.AreEquivalent(new List<string> { "The Email field is not a valid e-mail address." }, result.Errors["Email"]);
        }

        [TestMethod]
        public async Task Should_Create_User_When_Command_Is_Valid()
        {
            // Arrange
            var command = new AddAdminUserCommand()
            {
                UserName = "admin",
                FirstName = "admin",
                LastName = "admin",
                Email = "mevaajules9@gmail.com",
                Password = "12345678",
                PhoneNumber = ""
            };
            var uM = new UserModel
            {
                UserName = command.UserName,
                FirstName = command.FirstName,
                LastName = command.LastName,
                Email = command.Email
            };
            _repo.Setup(v => v.AddAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(uM);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);


            // Assert
            _repo.Verify(v => v.AddAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()));
            Assert.IsNotNull(result);
            Assert.AreEqual(result.UserName, uM.UserName);
            Assert.AreEqual(result.FirstName, uM.FirstName);
            Assert.AreEqual(result.LastName, uM.LastName);
            Assert.AreEqual(result.Email, uM.Email);
        }
    }
}
