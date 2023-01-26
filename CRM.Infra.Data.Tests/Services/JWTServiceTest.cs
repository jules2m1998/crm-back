using CRM.Core.Business.Models;
using CRM.Core.Domain.Entities;
using CRM.Infra.Services;
using Microsoft.Extensions.Configuration;
using Moq;

namespace CRM.Infra.Data.Tests.Services;

[TestClass]
public class JWTServiceTest
{
    private readonly JWTService _jWTService;
    private readonly Mock<IConfiguration> _configuration = new();

    public JWTServiceTest()
    {
        _jWTService = new JWTService(_configuration.Object);
    }

    [TestMethod]
    public void JWTService_Return_String_Token()
    {
        // Arrange
        var user = new User
        {
            UserName = "Test",
        };
        _configuration.Setup(c => c["Jwt:Issuer"]).Returns("");
        _configuration.Setup(c => c["Jwt:Audience"]).Returns("");
        _configuration.Setup(c => c["Jwt:Key"]).Returns("8to6cXIUHX3A1txtRNShGaNcHE-0wulnmx7jSsAiFC5Q4E2yQN7hKhY_u_W-2CbDJCsx6T-FKg-A15Rq5aOwNAz2GC1fxwOqhTZc0fr06BDtERRPDye95ihD1bOlLQxAxiBZmgjKqQQC7xjXgKHUzAKNajzmQZ3cbX39Ji3ITf0");

        // Act
        var result = _jWTService.Generate(user);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result is not null);
    }
}
