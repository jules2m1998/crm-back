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
