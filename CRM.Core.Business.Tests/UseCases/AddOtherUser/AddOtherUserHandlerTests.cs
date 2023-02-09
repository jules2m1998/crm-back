using CRM.Core.Business.Helpers;
using CRM.Core.Business.Models;
using CRM.Core.Business.Repositories;
using CRM.Core.Business.UseCases.AddOtherUser;
using CRM.Core.Domain;
using CRM.Core.Domain.Entities;
using CRM.Core.Domain.Exceptions;
using CRM.Core.Domain.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
        private readonly Mock<ISkillRepository> _skillRepositoryMock = new();

        public AddOtherUserHandlerTests()
        {
            _handler = new(_userRepositoryMock.Object, _fileHelperMock.Object, _skillRepositoryMock.Object);
            _skillRepositoryMock
                .Setup(s => s.AddRangeAsync(It.IsAny<IEnumerable<Skill>>()))
                .ReturnsAsync(new List<Skill>());
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
                }
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
                    Role = Roles.CLIENT
                }
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
            _userRepositoryMock
                .Setup(ur => ur.GetUserAndRole(It.IsAny<string>()))
                .ReturnsAsync(new Tuple<User, List<Role>>(new User(), new List<Role> { new Role { Name = Roles.ADMIN } }));
            var command = new AddOtherUserCommand
            {
                User = new UserBodyAndRole
                {
                    Role = Roles.SUPERVISOR
                }
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
                    Password = testStr,
                }
            };

            var user = new User();
            var role = new Role() { Name = Roles.ADMIN };
            _userRepositoryMock
                .Setup(ur => ur.GetUserAndRole(It.IsAny<string>()))
                .ReturnsAsync(new Tuple<User, List<Role>>(user, new List<Role>() { role}));
            var ur = new Tuple<User, List<Role>>(new User
            {
                UserName = testStr,
                FirstName = testStr,
                LastName = testStr,
                Email = testStr,
                Picture = "f1",
                PhoneNumber = testStr,
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

        [TestMethod]
        public async Task AddOtherUserHandler_Add_User_And_Skills()
        {
            // Arrange
            SkillModel skill = new()
            {
                IsCurrent = true,
                StartDate = DateTime.UtcNow,
                Name = "Studies",
            };
            SkillModel xp = new()
            {
                IsCurrent = true,
                StartDate = DateTime.UtcNow,
                Name = "Experiences",
            };
            var testStr = "mevaajules9@gmail.com";

            UserBodyAndRole user = new()
            {

                Role = Roles.SUPERVISOR,
                UserName = testStr,
                FirstName = testStr,
                LastName = testStr,
                Email = testStr,
                Picture = _fileMock.Object,
                PhoneNumber = testStr,
                Password = testStr,
                Studies = new List<SkillModel> {
                        skill
                    },
                Experiences = new List<SkillModel> {
                        xp
                    },
            };

            var command = new AddOtherUserCommand
            {
                CurrentUserName= "test",
                User = user
            };

            var u = new User();
            var role = new Role() { Name = Roles.ADMIN };
            _userRepositoryMock
                .Setup(ur => ur.GetUserAndRole(It.IsAny<string>()))
                .ReturnsAsync(new Tuple<User, List<Role>>(u, new List<Role>() { role }));
            var ur = new Tuple<User, List<Role>>(new User
            {
                UserName = testStr,
                FirstName = testStr,
                LastName = testStr,
                Email = testStr,
                Picture = "f1",
                PhoneNumber = testStr,
            }, new List<Role>() { new Role { Name = "test" } });
            _userRepositoryMock
                .Setup(ur => ur.AddAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new UserModel
                {
                    Experiences = user.Experiences,
                    Studies = user.Studies
                });
            _fileHelperMock
                .Setup(fs => fs.SaveImageToServerAsync(It.IsAny<IFormFile>(), It.IsAny<string[]>()))
                .ReturnsAsync(new Tuple<string, string>("f1", "f2"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Experiences);
            Assert.IsNotNull(result.Studies);

        }

        [TestMethod]
        public async Task AddOtherUserHandler_Throw_BaseException_If_StartDate_More_Recent_Than_End_Date()
        {
            // Arrange
            SkillModel skill = new()
            {
                IsCurrent = true,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(-1),
                Name = "Studies",
            };
            SkillModel xp = new()
            {
                IsCurrent = true,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(-1),
                Name = "Experiences",
            };

            UserBodyAndRole user = new()
            {
                UserName = "test",
                Email = "test@mail.com",
                FirstName = "test",
                LastName = "test",
                Role = Roles.CCL,
                Password = "12345678",
                Studies = new List<SkillModel> {
                        skill
                    },
                Experiences = new List<SkillModel> {
                        xp
                    },
            };
            var command = new AddOtherUserCommand
            {
                CurrentUserName = "test",
                User = user
            };

            var errors = new List<string>
            {
                "start date".ToGreaterThanMsg("end Date")
            };
            var testStr = "mevaajules9@gmail.com";

            var u = new User();
            var role = new Role() { Name = Roles.ADMIN };
            _userRepositoryMock
                .Setup(ur => ur.GetUserAndRole(It.IsAny<string>()))
                .ReturnsAsync(new Tuple<User, List<Role>>(u, new List<Role>() { role }));
            var ur = new Tuple<User, List<Role>>(new User
            {
                UserName = testStr,
                FirstName = testStr,
                LastName = testStr,
                Email = testStr,
                Picture = "f1",
                PhoneNumber = testStr,
            }, new List<Role>() { new Role { Name = "test" } });
            _userRepositoryMock
                .Setup(ur => ur.AddAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new UserModel());
            _fileHelperMock
                .Setup(fs => fs.SaveImageToServerAsync(It.IsAny<IFormFile>(), It.IsAny<string[]>()))
                .ReturnsAsync(new Tuple<string, string>("f1", "f2"));

            // Act
            var result = await Assert
                .ThrowsExceptionAsync<BaseException>(() => _handler.Handle(command, CancellationToken.None));
            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BaseException));
            CollectionAssert.AreEquivalent(result.Errors["Studies[0]"], errors);
            CollectionAssert.AreEquivalent(result.Errors["Experiences[0]"], errors);
        }
    }
}
