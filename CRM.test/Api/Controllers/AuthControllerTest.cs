using CRM.api.Controllers;
using CRM.core.Models;
using CRM.core.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CRM.test.Api.Controllers
{
    [TestClass]
    public class AuthControllerTest
    {
        private readonly Mock<IJWTService> jWTServiceMock = new();
        private readonly AuthController authController;

        public AuthControllerTest()
        {
            authController = new(jWTServiceMock.Object);
        }
        [TestMethod]
        public void AuthController_CreateToken_IsActionResultResult()
        {
            // Arrange
            var u = new User();
            jWTServiceMock
                .Setup(m => m.GenerateToken(u))
                .Returns(string.Empty);

            // Act
            var result = authController.CreateToken(u);
            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result is ActionResult<string>);
        }
    }
}
