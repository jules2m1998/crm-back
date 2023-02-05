using CRM.Core.Business.Helpers;
using CRM.Core.Business.Models;
using CRM.Core.Business.Repositories;
using CRM.Core.Business.UseCases.AddOtherUser;
using CRM.Core.Domain;
using CRM.Core.Domain.Entities;
using CRM.Core.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.Tests.UseCases.AddOtherUser
{
    [TestClass]
    public class AddOtherUserHandlerTests
    {
        private readonly AddOtherUserHandler _handler;
        private readonly Mock<IUserRepository> _userRepositoryMock = new();
        private readonly Mock<IFileHelper> _fileHelperMock = new();
        private readonly Mock<IFormFile> _fileMock = new();

        public AddOtherUserHandlerTests()
        {
            _handler = new(_userRepositoryMock.Object, _fileHelperMock.Object);
        }

        [TestMethod]
        public async Task AddOtherUserHandler_Throw_Unauthorise_If_Current_User_Is_Supervisor_And_Create_Admin()
        {
            // Arrange
            var command = new AddOtherUserCommand
            {
                User = new UserBodyAndRole
                {
                    Role = Roles.ADMIN
                },
                Roles = new List<string> { Roles.SUPERVISOR }
            };


            // Act
            var result = await Assert.ThrowsExceptionAsync<UnauthorizedAccessException>(async () => await _handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(UnauthorizedAccessException));
            _userRepositoryMock.Verify(ur => ur.AddAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _fileHelperMock.Verify(fh => fh.SaveImageToServerAsync(It.IsAny<IFormFile>(), It.IsAny<string[]>()), Times.Never);
        }

        [TestMethod]
        public async Task AddOtherUserHandler_Throw_Unauthorise_If_Current_User_Is_Client_And_Create_Admin()
        {
            // Arrange
            var command = new AddOtherUserCommand
            {
                User = new UserBodyAndRole
                {
                    Role = Roles.CLIIENT
                },
                Roles = new List<string> { Roles.CLIIENT }
            };


            // Act
            var result = await Assert.ThrowsExceptionAsync<UnauthorizedAccessException>(async () => await _handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(UnauthorizedAccessException));
            _userRepositoryMock.Verify(ur => ur.AddAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _fileHelperMock.Verify(fh => fh.SaveImageToServerAsync(It.IsAny<IFormFile>(), It.IsAny<string[]>()), Times.Never);
        }

        [TestMethod]
        public async Task AddOtherUserHandler_Throw_BaseException_IF_User_Invalid()
        {
            // Arrange
            Mock<Tuple<User, List<Role>>> userRole = new();
            var command = new AddOtherUserCommand
            {
                User = new UserBodyAndRole
                {
                    Role = Roles.SUPERVISOR
                },
                Roles = new List<string> { Roles.ADMIN }
            };


            // Act
            var result = await Assert.ThrowsExceptionAsync<BaseException>(async () => await _handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BaseException));
            _fileHelperMock.Verify(fh => fh.SaveImageToServerAsync(It.IsAny<IFormFile>(), It.IsAny<string[]>()), Times.Never);
        }

        [TestMethod]
        public async Task AddOtherUserHandler_Add_User_When_Valid_And_Good_Role()
        {
            // Arrange
            var testStr = "mevaajules9@gmail.com";
            var command = new AddOtherUserCommand
            {
                User = new UserBodyAndRole
                {
                    Role = Roles.SUPERVISOR,
                    UserName = testStr,
                    FirstName = testStr,
                    LastName = testStr,
                    Email = testStr,
                    Picture = _fileMock.Object,
                    PhoneNumber = testStr,
                },
                Roles = new List<string> { Roles.ADMIN }
            };
            var ur = new Tuple<User, List<Role>>(new User
            {
                UserName = testStr,
                FirstName = testStr,
                LastName = testStr,
                Email = testStr,
                Picture = "f1",
                PhoneNumber = testStr
            }, new List<Role>() { new Role { Name= "test"} });
            _userRepositoryMock
                .Setup(ur => ur.AddAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new UserModel());
            _fileHelperMock
                .Setup(fs => fs.SaveImageToServerAsync(It.IsAny<IFormFile>(), It.IsAny<string[]>()))
                .ReturnsAsync(new Tuple<string, string>("f1", "f2"));


            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(UserModel));
            _userRepositoryMock.Verify(ur => ur.AddAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            _fileHelperMock.Verify(fh => fh.SaveImageToServerAsync(It.IsAny<IFormFile>(), It.IsAny<string[]>()), Times.Once);
        }
    }
}
