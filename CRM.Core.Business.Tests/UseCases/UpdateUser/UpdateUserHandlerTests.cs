using CRM.Core.Business.Helpers;
using CRM.Core.Business.Models;
using CRM.Core.Business.Repositories;
using CRM.Core.Business.UseCases.UpdateUser;
using CRM.Core.Domain;
using CRM.Core.Domain.Entities;
using CRM.Core.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.Tests.UseCases.UpdateUser
{
    [TestClass]
    public class UpdateUserHandlerTests
    {
        private readonly UpdateUserHandler _handler;
        private readonly Mock<IUserRepository> _userRepositoryMock = new();
        private readonly Mock<IFileHelper> _fileHelperMock = new();

        public UpdateUserHandlerTests()
        {
            _handler = new(_userRepositoryMock.Object, _fileHelperMock.Object);
        }

        [TestMethod]
        public async Task UpdateUserHandler_Handle_Throw_BadRequest_When_Username_Empty()
        {
            // Arrange
            var cmd = new UpdateUserCommand { };

            // Act
            var result = await Assert.ThrowsExceptionAsync<BaseException>(() => _handler.Handle(cmd, CancellationToken.None));

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BaseException));
            CollectionAssert.AreEquivalent(result.Errors["UserName"], new List<string> { "This field is required !" });
            _userRepositoryMock.Verify(u => u.UpdateUserAsync(It.IsAny<User>()), Times.Never);
            _fileHelperMock.Verify(f => f.DeleteImageToServer(It.IsAny<string>()), Times.Never);
            _fileHelperMock.Verify(f => f.SaveImageToServerAsync(It.IsAny<IFormFile>(), It.IsAny<string[]>()), Times.Never);
            _userRepositoryMock.Verify(f => f.GetUserAndRolesAsync(It.IsAny<Guid>(), It.IsAny<string>()), Times.Never);
            _userRepositoryMock.Verify(f => f.GetUserAndRolesAsync(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task UpdateUserHandler_Throw_BaseException_When_Password_Is_Set_But_ConfirmPassword_Is_Empty_And_User()
        {
            // Arrange
            SetValidUser(out UpdateUserCommand cmd, "tester");
            _userRepositoryMock
                .Setup(u => u.GetUserAndRolesAsync(It.IsAny<Guid>(), It.IsAny<string>()))
                .ReturnsAsync(new User()
                {
                    UserName = "Teste"
                });

            // Act
            var result = await Assert.ThrowsExceptionAsync<BaseException>(() => _handler.Handle(cmd, CancellationToken.None));

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BaseException));
            CollectionAssert.AreEquivalent(result.Errors["Password"], new List<string> { "Password not confirmed !" });
            _userRepositoryMock.Verify(u => u.UpdateUserAsync(It.IsAny<User>()), Times.Never);
            _fileHelperMock.Verify(f => f.DeleteImageToServer(It.IsAny<string>()), Times.Never);
            _fileHelperMock.Verify(f => f.SaveImageToServerAsync(It.IsAny<IFormFile>(), It.IsAny<string[]>()), Times.Never);
            _userRepositoryMock.Verify(f => f.GetUserAndRolesAsync(It.IsAny<Guid>(), It.IsAny<string>()), Times.Never);
            _userRepositoryMock.Verify(f => f.GetUserAndRolesAsync(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task UpdateUserHandler_Handle_Null_If_User_Not_Found()
        {
            // Arrange
            SetValidUser(out UpdateUserCommand cmd);
            User? user = null;
            _userRepositoryMock
                .Setup(u => u.GetUserAndRolesAsync(It.IsAny<Guid>(), It.IsAny<string>()))
                .ReturnsAsync(user);

            // Act
            var result = await _handler.Handle(cmd, CancellationToken.None);

            // Assert
            Assert.IsNull(result);
            _fileHelperMock.Verify(f => f.DeleteImageToServer(It.IsAny<string>()), Times.Never);
            _fileHelperMock.Verify(f => f.SaveImageToServerAsync(It.IsAny<IFormFile>(), It.IsAny<string[]>()), Times.Never);
            _userRepositoryMock.Verify(u => u.UpdateUserAsync(It.IsAny<User>()), Times.Never);
            _userRepositoryMock.Verify(f => f.GetUserAndRolesAsync(It.IsAny<Guid>(), It.IsAny<string>()), Times.Once);
            _userRepositoryMock.Verify(f => f.GetUserAndRolesAsync(It.IsAny<string>()), Times.Never);
        }

        private static void SetValidUser(out UpdateUserCommand cmd, string? npwd = null, string? oldPwd = null)
        {
            var bytes = Encoding.UTF8.GetBytes("This is a dummy file");
            IFormFile file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "dummy.png");
            var text = "test";
            cmd = new UpdateUserCommand
            {
                User = new(Guid.NewGuid(), text, "mail@mail.com", text, text, new List<string> { Roles.ADMIN }, file, null, npwd, oldPwd, null, null),
                UserName = "test"
            };
        }
    }
}
