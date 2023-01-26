using CRM.Infra.Data.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Infra.Data.Tests.Helpers
{
    [TestClass]
    public class FileHelperTests
    {
        private readonly FileHelper _helper;
        private readonly Mock<IFormFile> _formFile = new();
        private readonly Mock<IWebHostEnvironment> _environnement = new();

        public FileHelperTests()
        {
            _helper = new FileHelper(_environnement.Object);
        }

        [TestMethod]
        public async Task FileHelper_Return_File_Full_Path()
        {
            // Arrange
            _environnement.SetupGet(e => e.WebRootPath).Returns("/");
            _formFile.SetupGet(f => f.FileName).Returns("test.jpg");

            // Act
            var result = await _helper.SaveImageToServerAsync(_formFile.Object, new []{ "img", "test" });
            var dbName = result.Item1 as string;
            var fullName = result.Item2 as string;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(File.Exists(result.Item2));
            Assert.IsTrue(dbName.EndsWith(".jpg"));
            Assert.IsTrue(fullName.EndsWith(".jpg"));
            File.Delete(result.Item2);
            Assert.IsFalse(File.Exists(result.Item2));
        }

        [TestMethod]
        public async Task FileHelper_Delete_File()
        {
            // Arrange
            _environnement.SetupGet(e => e.WebRootPath).Returns("/");
            _formFile.SetupGet(f => f.FileName).Returns("test.jpg");

            // Act
            var result = await _helper.SaveImageToServerAsync(_formFile.Object, new[] { "img", "test" });
            var dbName = result.Item1;
            _helper.DeleteImageToServer(dbName);

            // Assert
            Assert.IsFalse(File.Exists(result.Item1));
        }
    }
}
