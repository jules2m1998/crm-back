using CRM.Core.Business.Helpers;
using CRM.Core.Domain.Exceptions;
using CRM.Core.Domain.Extensions;
using CRM.Infra.Data.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using System.ComponentModel.DataAnnotations;
using System.Text;
using CsvHelper.Configuration;

namespace CRM.Infra.Data.Tests.Helpers;

class TestTypeFileReadCsv : IFileReadable
{
    [Required]
    public int? Id { get; set; }
    [MinLength(5)]
    public string? LastName { get; set; } = string.Empty;
    public FIleReadStatus Status { get; set; } = FIleReadStatus.Valid;
    public Dictionary<string, List<string>>? Errors { get; set; }
}

class TestTypeFileReadCsvClassMap : ClassMap<TestTypeFileReadCsv>
{
    public TestTypeFileReadCsvClassMap()
    {
        Map(m => m.Id).Name("id");
        Map(m => m.LastName).Name("lastname");
    }
}

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
        var dbName = result.Item1;
        var fullName = result.Item2;

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

    [TestMethod]
    public void FileHelper_ReadCsvFile_Throw_BaseException_If_IFormFile_Is_Not_CSVFile()
    {
        // Arrange
        var bytes = Encoding.UTF8.GetBytes("This is a dummy file");
        IFormFile file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "dummy.txt");

        // Act
        var result = Assert.ThrowsException<BadHttpRequestException>(() => _helper.ReadCsvFile<TestTypeFileReadCsv, TestTypeFileReadCsvClassMap>(file));

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(BadHttpRequestException));
    }

    [TestMethod]
    public void FileHelper_ReadCsvFile_Return_List_Of_Model_If_Is_CSVFile()
    {
        // Arrange
        var bytes = Encoding.UTF8.GetBytes("id;lastname\r\n100;Halsey\r\n101;Sabella\r\n102;Vanni\r\n103;Sidonius\r\n104;Norvol\r\n105;Tremayne\r\n106;Redmond\r\n107;Atonsah\r\n108;Land\r\n109;Ardra");
        IFormFile file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "dummy.csv");

        // Act
        var result = _helper.ReadCsvFile<TestTypeFileReadCsv, TestTypeFileReadCsvClassMap>(file);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(10, result.Count);
        Assert.IsInstanceOfType(result, typeof(List<TestTypeFileReadCsv>));
    }

    [TestMethod]
    public void FileHelper_ReadCsvFile_Verify_Return_Invalid_Object_In_List()
    {
        // Arrange
        var bytes = Encoding.UTF8.GetBytes("id;lastname\r\n;Hal");
        IFormFile file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "dummy.csv");
        IDictionary<string, List<string>> errors = new Dictionary<string, List<string>>();
        errors.Add("Id", new List<string> { "The Id field is required." });
        errors.Add("LastName", new List<string> { "The field LastName must be a string or array type with a minimum length of '5'." });

        // Act
        var result = _helper.ReadCsvFile<TestTypeFileReadCsv, TestTypeFileReadCsvClassMap>(file);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(List<TestTypeFileReadCsv>));
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual(FIleReadStatus.Invalid, result[0].Status);
        Assert.AreEqual("Hal", result[0].LastName);
        Assert.AreEqual(null, result[0].Id);
        CollectionAssert.AreEquivalent(errors["Id"], result[0].Errors["Id"].ToList());
        CollectionAssert.AreEquivalent(errors["LastName"], result[0].Errors["LastName"].ToList());
    }

    [TestMethod]
    public void FileHelper_ReadCsvFile_Verify_Return_Valid_Object_In_List()
    {
        // Arrange
        var bytes = Encoding.UTF8.GetBytes("id;lastname\r\n100;Sabella");
        IFormFile file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "dummy.csv");

        // Act
        var result = _helper.ReadCsvFile<TestTypeFileReadCsv, TestTypeFileReadCsvClassMap>(file);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(List<TestTypeFileReadCsv>));
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual(FIleReadStatus.Valid, result[0].Status);
        Assert.AreEqual(100, result[0].Id);
        Assert.AreEqual("Sabella", result[0].LastName);

    }

    [TestMethod]
    public void FileHelper_ReadCsvFile_Verify_Return_Valid_And_Invalid_Object_In_List()
    {
        // Arrange
        var bytes = Encoding.UTF8.GetBytes("id;lastname\r\n100;Rooney\r\n;Ke");
        IFormFile file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "dummy.csv");
        IDictionary<string, List<string>> errors = new Dictionary<string, List<string>>();
        errors.Add("Id", new List<string> { "Id".ToRequiredMsg() });
        errors.Add("LastName", new List<string> { "LastName".ToInvalidMsg() });

        // Act
        var result = _helper.ReadCsvFile<TestTypeFileReadCsv, TestTypeFileReadCsvClassMap>(file).ToList();

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(List<TestTypeFileReadCsv>));
        Assert.AreEqual(2, result.Count);
        Assert.AreEqual(FIleReadStatus.Valid, result[0].Status);
        Assert.AreEqual(FIleReadStatus.Invalid, result[1].Status);
        Assert.AreEqual(100, result[0].Id);
        Assert.AreEqual("Rooney", result[0].LastName);
        Assert.IsNull(result[1].Id);
        Assert.AreEqual("Ke", result[1].LastName);
    }
}
