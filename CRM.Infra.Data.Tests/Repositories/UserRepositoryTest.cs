﻿using CRM.Core.Business.Models;
using CRM.Core.Domain.Entities;
using CRM.Core.Domain.Exceptions;
using CRM.Core.Domain.Extensions;
using CRM.Infra.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace CRM.Infra.Data.Tests.Repositories;

[TestClass]
public class UserRepositoryTest
{
    private readonly UserRepository _repo;
    private readonly Mock<IUserStore<User>> _store = new();
    private readonly Mock<IRoleStore<Role>> _storeRole = new();
    private readonly Mock<UserManager<User>> _userManager;
    private readonly Mock<RoleManager<Role>> _roleManager;

    public UserRepositoryTest()
    {
        _userManager = new(_store.Object, null, null, null, null, null, null, null, null);
        _roleManager = new(_storeRole.Object, null, null, null, null);
        _repo = new(_userManager.Object, _roleManager.Object);
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
            Picture = "Pic",
            PhoneNumber = "phone",
            CreatedAt= DateTime.Now,
        };
        var role = "Test";
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
        Assert.AreEqual(user.Picture, result.Picture);
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
        var role = "Teste";
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
}