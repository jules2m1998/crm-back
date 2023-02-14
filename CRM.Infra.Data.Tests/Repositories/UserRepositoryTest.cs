using CRM.Core.Business.Helpers;
using CRM.Core.Business.Models;
using CRM.Core.Domain;
using CRM.Core.Domain.Entities;
using CRM.Core.Domain.Exceptions;
using CRM.Core.Domain.Extensions;
using CRM.Infra.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Data;

namespace CRM.Infra.Data.Tests.Repositories;

[TestClass]
public class UserRepositoryTest
{
    private readonly UserRepository _repo;
    private readonly Mock<IUserStore<User>> _store = new();
    private readonly Mock<IRoleStore<Role>> _storeRole = new();
    private readonly Mock<IFileHelper> _fileHelper = new();
    private readonly Mock<UserManager<User>> _userManager;
    private readonly Mock<RoleManager<Role>> _roleManager;

    public UserRepositoryTest()
    {
        _userManager = new(_store.Object, null, null, null, null, null, null, null, null);
        _roleManager = new(_storeRole.Object, null, null, null, null);
        _repo = new(_userManager.Object, _roleManager.Object, _fileHelper.Object);
    }

    [TestMethod]
    public async Task User_Repository_Return_Not_Null_Usermodel()
    {
        // Arrange
        var user = new User()
        {
            Id= Guid.NewGuid(),
            UserName = "Username",
            Email = "Email",
            FirstName = "Firstname",
            LastName = "LastName",
            PhoneNumber = "phone",
            CreatedAt= DateTime.Now,
        };
        var role = Roles.CCL;
        var pwd = "";
        _userManager
            .Setup(s => s.CreateAsync(user, pwd))
            .ReturnsAsync(IdentityResult.Success);
        _userManager
            .Setup(u => u.AddToRoleAsync(user, role))
            .ReturnsAsync(IdentityResult.Success);
        _userManager
            .Setup(u => u.FindByNameAsync(user.UserName))
            .ReturnsAsync(user);

        // Act
        var result = await _repo.AddAsync(user, pwd, role);

        // Assert
        _userManager.Verify(d => d.CreateAsync(user, pwd), Times.Once);
        Assert.IsNotNull(result);
        Assert.AreEqual(user.UserName, result.UserName);
        Assert.AreEqual(user.Email, result.Email);
        Assert.AreEqual(user.FirstName, result.FirstName);
        Assert.AreEqual(user.LastName, result.LastName);
        Assert.AreEqual("default\\default-user.png", result.Picture);
        Assert.AreEqual(user.PhoneNumber, result.PhoneNumber);
        Assert.AreEqual(user.CreatedAt.Ticks, result.CreatedAt.Ticks);
        Assert.AreEqual(user.UpdateAt?.Ticks, result.UpdateAt?.Ticks);
        Assert.AreEqual(user.DeletedAt?.Ticks, result.DeletedAt?.Ticks);
        Assert.IsInstanceOfType(result, typeof(UserModel));
        CollectionAssert.AreEquivalent(result.Roles, new List<string> { role });
    }

    [TestMethod]
    public async Task User_Repository_Throw_BaseException_When_UserManager_Success_False()
    {
        // Arrange
        var user = new User();
        var role = "CCL";
        var pwd = "";
        var idResult = new IdentityResult();
        _userManager
            .Setup(s => s.CreateAsync(user, pwd))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError[]
            {
                new IdentityError() { Code = "DuplicateEmail" },
                new IdentityError() { Code = "InvalidEmail" },
                new IdentityError() { Code = "DuplicateUserName" },
            }));
        var expectR = new Dictionary<string, List<string>> {
            { "Email",  new List<string>{
                "email".ToAlReadyExistMsg() ,
                "email".ToInvalidMsg()  } },
            { "UserName", new List<string>{ "username".ToAlReadyExistMsg() }},
        };

        // Act
        var result = await Assert.ThrowsExceptionAsync<BaseException>(() => _repo.AddAsync(user, pwd, role));

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(BaseException));
        Assert.AreEqual(expectR.Count, result.Errors.Count);
        CollectionAssert.AreEquivalent(expectR["Email"], result.Errors["Email"]);
        CollectionAssert.AreEquivalent(expectR["UserName"], result.Errors["UserName"]);
    }



    [TestMethod]
    public async Task UserRepository_GetByUserAndRoleAsync_Return_Null_If_User_Not_Exist()
    {
        User? u = null;
        // Arranche
        _userManager.Setup(u => u.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(u);

        // Act
        var result = await _repo.GetByUserAndRoleAsync(It.IsAny<string>(), It.IsAny<string>());

        // Assert
        _userManager.Verify(v => v.FindByNameAsync(It.IsAny<string>()), Times.Once);
        _userManager.Verify(v => v.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Never);
        _userManager.Verify(r => r.GetRolesAsync(It.IsAny<User>()), Times.Never);
        Assert.IsNull(result);
    }

    [TestMethod]
    public async Task UserRepository_GetByUserAndRoleAsync_Return_Null_If_Wrong_PWD()
    {
        Mock<User> u = new();
        // Arranche
        _userManager
            .Setup(u => u.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(u.Object);
        _userManager
            .Setup(u => u.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(false);

        // Act
        var result = await _repo.GetByUserAndRoleAsync(It.IsAny<string>(), It.IsAny<string>());

        // Assert
        _userManager.Verify(v => v.FindByNameAsync(It.IsAny<string>()), Times.Once);
        _userManager.Verify(v => v.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Once);
        _userManager.Verify(r => r.GetRolesAsync(It.IsAny<User>()), Times.Never);
        Assert.IsNull(result);
    }

    [TestMethod]
    public async Task UserRepository_GetByUserAndRoleAsync_Return_User_If_Correct_Data()
    {
        var user = new User
        {
            UserName = "test"
        };
        var roles = new List<string> { "Admin" };
        var role = new Role { Name = "test" };
        var rolesToReturn = new List<Role> { role };

        // Arrange
        _userManager
            .Setup(u => u.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(user);
        _userManager
            .Setup(u => u.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(true);
        _userManager
            .Setup(u => u.GetRolesAsync(It.IsAny<User>()))
            .ReturnsAsync(roles);
        _roleManager
            .Setup(r => r.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(role);

        // Act
        var result = await _repo.GetByUserAndRoleAsync(user.UserName, It.IsAny<string>());

        // Assert
        _userManager.Verify(v => v.FindByNameAsync(It.IsAny<string>()), Times.Once);
        _userManager.Verify(v => v.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Once);
        _userManager.Verify(r => r.GetRolesAsync(It.IsAny<User>()), Times.Once);
        Assert.IsNotNull(result.Item1);
        Assert.IsNotNull(result.Item2);
        Assert.AreEqual(user.UserName, result.Item1.UserName);
        CollectionAssert.AreEquivalent(rolesToReturn, result.Item2);

    }

    [TestMethod]
    public async Task UseerRepository_GetUserAndRole_UserName_Return_Null_If_User_Not_Found()
    {
        // Arrange
        User? user = null;
        var username = "Test";
        _userManager.Setup(u => u.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(user);

        // Act
        var userAndRoles = await _repo.GetUserAndRole(username);

        // Assert
        _userManager.Verify(u => u.FindByNameAsync(It.IsAny<string>()), Times.Once);
        _userManager.Verify(v => v.GetRolesAsync(It.IsAny<User>()), Times.Never);
        _roleManager.Verify(v => v.FindByNameAsync(It.IsAny<string>()), Times.Never);
        Assert.IsNull(userAndRoles);
    }

    [TestMethod]
    public async Task UseerRepository_GetUserAndRole_UserName_Return_User_Alright()
    {
        // Arrange
        User? user = new User();
        var username = "Test";
        var role = new Role { Name = "test" };
        var roles = new List<string> { "Admin" };
        _userManager.Setup(u => u.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(user);
        _userManager.Setup(u => u.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(roles);
        _roleManager
            .Setup(r => r.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(role);

        // Act
        var userAndRoles = await _repo.GetUserAndRole(username);

        // Assert
        _userManager.Verify(u => u.FindByNameAsync(It.IsAny<string>()), Times.Once);
        _userManager.Verify(v => v.GetRolesAsync(It.IsAny<User>()), Times.Once);
        _roleManager.Verify(v => v.FindByNameAsync(It.IsAny<string>()), Times.AtMost(1));
        Assert.IsNotNull(userAndRoles);
    }



    [TestMethod]
    public async Task UserRepository_AddFromListAsync_Throw_UnauthorizedAccessException_When_Current_User_Is_Client()
    {
        // Arrange

        var listCsv = new List<UserCsvModel>()
        {
            new UserCsvModel {Status = FIleReadStatus.Invalid}
        };
        var user = new User();
        var role = new Role() { Name = Roles.CLIENT };
        _userManager
            .Setup(ur => ur.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(user);
        _userManager
            .Setup(ur => ur.GetRolesAsync(It.IsAny<User>()))
            .ReturnsAsync(new List<string>() { Roles.CLIENT });
        _roleManager
            .Setup(rm => rm.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(role);

        // Act
        var result = await Assert.ThrowsExceptionAsync<UnauthorizedAccessException>(() => _repo.AddFromListAsync(listCsv, Roles.CLIENT, "test"));

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(UnauthorizedAccessException));
    }



    [TestMethod]
    public async Task UserRepository_AddFromListAsync_Throw_UnauthorizedAccessException_When_Current_User_Is_Supervisor_And_Role_Is_Admin()
    {
        // Arrange

        var listCsv = new List<UserCsvModel>()
        {
            new UserCsvModel {Status = FIleReadStatus.Invalid}
        };
        var user = new User();
        var role = new Role() { Name = Roles.SUPERVISOR };
        _userManager
            .Setup(ur => ur.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(user);
        _userManager
            .Setup(ur => ur.GetRolesAsync(It.IsAny<User>()))
            .ReturnsAsync(new List<string>() { Roles.SUPERVISOR });
        _roleManager
            .Setup(rm => rm.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(role);

        // Act
        var result = await Assert.ThrowsExceptionAsync<UnauthorizedAccessException>(() => _repo.AddFromListAsync(listCsv, Roles.ADMIN, "test"));

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(UnauthorizedAccessException));
    }

    [TestMethod]
    public async Task UserRepository_AddFromListAsync_Throw_UnauthorizedAccessException_When_Current_User_Is_Supervisor_And_Role_Is_Supervisor()
    {
        // Arrange

        var listCsv = new List<UserCsvModel>()
        {
            new UserCsvModel {Status = FIleReadStatus.Invalid}
        };
        var user = new User();
        var role = new Role() { Name = Roles.SUPERVISOR };
        _userManager
            .Setup(ur => ur.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(user);
        _userManager
            .Setup(ur => ur.GetRolesAsync(It.IsAny<User>()))
            .ReturnsAsync(new List<string>() { Roles.SUPERVISOR });
        _roleManager
            .Setup(rm => rm.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(role);

        // Act
        var result = await Assert.ThrowsExceptionAsync<UnauthorizedAccessException>(() => _repo.AddFromListAsync(listCsv, Roles.SUPERVISOR, "test"));

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(UnauthorizedAccessException));
    }

    [TestMethod]
    public async Task UserRepository_AddFromListAsync_Throw_UnauthorizedAccessException_When_Current_User_Is_CCL_And_Role_Is_Supervisor()
    {
        // Arrange

        var listCsv = new List<UserCsvModel>()
        {
            new UserCsvModel {Status = FIleReadStatus.Invalid}
        };
        var user = new User();
        var role = new Role() { Name = Roles.CCL };
        _userManager
            .Setup(ur => ur.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(user);
        _userManager
            .Setup(ur => ur.GetRolesAsync(It.IsAny<User>()))
            .ReturnsAsync(new List<string>() { Roles.CCL });
        _roleManager
            .Setup(rm => rm.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(role);

        // Act
        var result = await Assert.ThrowsExceptionAsync<UnauthorizedAccessException>(() => _repo.AddFromListAsync(listCsv, Roles.SUPERVISOR, "test"));

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(UnauthorizedAccessException));
    }

    [TestMethod]
    public async Task UserRepository_AddFromListAsync_Throw_UnauthorizedAccessException_When_Current_User_Is_CCL_And_Role_Is_ADMIN()
    {
        // Arrange

        var listCsv = new List<UserCsvModel>()
        {
            new UserCsvModel {Status = FIleReadStatus.Invalid}
        };
        var user = new User();
        var role = new Role() { Name = Roles.CCL };
        _userManager
            .Setup(ur => ur.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(user);
        _userManager
            .Setup(ur => ur.GetRolesAsync(It.IsAny<User>()))
            .ReturnsAsync(new List<string>() { Roles.CCL });
        _roleManager
            .Setup(rm => rm.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(role);

        // Act
        var result = await Assert.ThrowsExceptionAsync<UnauthorizedAccessException>(() => _repo.AddFromListAsync(listCsv, Roles.CCL, "test"));

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(UnauthorizedAccessException));
    }

    [TestMethod]
    public async Task UserRepository_AddFromListAsync_Save_When_Current_User_Is_Supervisor_And_Role_Is_CCL()
    {
        // Arrange

        var listCsv = new List<UserCsvModel>()
        {
            new UserCsvModel {Status = FIleReadStatus.Invalid}
        };
        var user = new User();
        var role = new Role() { Name = Roles.SUPERVISOR };
        _userManager
            .Setup(ur => ur.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(user);
        _userManager
            .Setup(ur => ur.GetRolesAsync(It.IsAny<User>()))
            .ReturnsAsync(new List<string>() { Roles.SUPERVISOR });
        _roleManager
            .Setup(rm => rm.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(role);

        // Act
        var result = await _repo.AddFromListAsync(listCsv, Roles.CCL, "test");

        // Assert
        Assert.IsNotNull(result);
        Assert.IsNotInstanceOfType(result, typeof(UnauthorizedAccessException));
    }

    [TestMethod]
    public async Task UseerRepository_AddFromListAsync_Not_Set_Invalid_Status()
    {
        // Arrange
        var listCsv = new List<UserCsvModel>()
        {
            new UserCsvModel {Status = FIleReadStatus.Invalid}
        };
        var user = new User();
        var role = new Role() { Name = Roles.ADMIN};
        _userManager
            .Setup(ur => ur.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(user);
        _userManager
            .Setup(ur => ur.GetRolesAsync(It.IsAny<User>()))
            .ReturnsAsync(new List<string>());
        _roleManager
            .Setup(rm => rm.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(role);


        // Act
        var result = await _repo.AddFromListAsync(listCsv, "CCL", "admin");


        // Assert
        _userManager.Verify(u => u.CreateAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Never);
        _userManager.Verify(u => u.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Never);
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual(result[0].Status, FIleReadStatus.Invalid);
    }

    [TestMethod]
    public async Task UseerRepository_AddFromListAsync_Set_Valid_Status_To_Invalid_Status()
    {
        // Arrange
        var errors = new Dictionary<string, List<string>>()
        {
            {"test", new List<string>(){ "test" } }
        };
        var role = new Role { Name = "test" };
        var roles = new List<string> { "Admin" };
        var user = new User();
        _userManager
            .Setup(ur => ur.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(user);
        _userManager
            .Setup(ur => ur.GetRolesAsync(It.IsAny<User>()))
            .ReturnsAsync(new List<string>());
        _roleManager
            .Setup(rm => rm.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(role);
        _userManager.Setup(u => u.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .Throws(new BaseException(errors));
        var listCsv = new List<UserCsvModel>()
        {
            new UserCsvModel {Status = FIleReadStatus.Valid}
        };


        // Act
        var result = await _repo.AddFromListAsync(listCsv, "CCL", "");


        // Assert
        _userManager.Verify(u => u.CreateAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Once);
        _userManager.Verify(u => u.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Never);
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual(result[0].Status, FIleReadStatus.Exist);
    }

    // Get user by user creator name tests
    [TestMethod]
    public async Task UserRepository_GetUsersByCreatorAsync_Throw_Unautorized_Exception_When_Creator_Not_Exist()
    {
        // Arrange
        var creatoUserName = "Test";
        User? user = null;
        _userManager
            .Setup(u => u.FindByNameAsync(creatoUserName))
            .ReturnsAsync(user);

        // Act
        var result = await Assert.ThrowsExceptionAsync<UnauthorizedAccessException>(() => _repo.GetUsersByCreatorUserNameAsync(creatoUserName));

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(UnauthorizedAccessException));
        _userManager.Verify(u => u.FindByNameAsync(creatoUserName), Times.Once);
    }
}
