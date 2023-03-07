using Microsoft.AspNetCore.Http;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

namespace CRM.Core.Business.Helpers
{
    public interface IFileHelper
    {
        string GetUniqueFileName(string fileName);
        /// <summary>
        /// Method <c>SaveImageToServerAsync</c> Create image to http web server.
        /// <example>
        /// <code>
        /// var fileResult = await _helper.SaveImageToServerAsync(formFile, "exemple.jpg", new []{ "img", "exemples" });
        /// var pathToDb = fileResult.Item1;
        /// results in <c>fileResult</c>'s having the value ("img\\test\\test.jpg", "{serverFilePath}\\img\\test\\test.jpg").
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="formFile">Form file to save to the server.</param>
        /// <param name="directory">Directory tree to store file.</param>
        /// <returns>
        /// A tuple of two representing path name to the database and path name to retrieve file to the server
        /// </returns>
        Task<Tuple<string, string>> SaveImageToServerAsync(IFormFile formFile, string[] directory);
        void DeleteImageToServer(string fileName);
        List<Return> ReadCsvFile<Return, Mapper>(IFormFile file) where Return: IFileReadable where Mapper: ClassMap<Return>;
    }
}
