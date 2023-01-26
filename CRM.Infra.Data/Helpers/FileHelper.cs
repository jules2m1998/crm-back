using CRM.Core.Business.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace CRM.Infra.Data.Helpers
{
    public class FileHelper : IFileHelper
    {
        private readonly IWebHostEnvironment _environment;

        public FileHelper(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        /// <summary>
        /// Delete file to the server if exist
        /// </summary>
        /// <param name="fileName">Relative file path</param>
        public void DeleteImageToServer(string fileName)
        {
            var fullPath = Path.Combine(_environment.WebRootPath, fileName);
            if(File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }

        public string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);

            return string.Concat(Path.GetFileNameWithoutExtension(fileName)
                                , "_"
                                , Guid.NewGuid().ToString().AsSpan(0, 4)
                                , Path.GetExtension(fileName));
        }

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
        public async Task<Tuple<string, string>> SaveImageToServerAsync(IFormFile formFile, string[] directory)
        {
            var fileName = GetUniqueFileName(formFile.FileName);
            var folderName = Path.Combine(directory);
            var pathToSave = Path.Combine(_environment.WebRootPath, folderName);
            var dbPath = Path.Combine(folderName, fileName);
            if(!Directory.Exists(pathToSave)) Directory.CreateDirectory(pathToSave);
            pathToSave= Path.Combine(pathToSave, fileName);

            using var stream = new FileStream(pathToSave, FileMode.Create);
            await formFile.CopyToAsync(stream);
            return new Tuple<string, string>(dbPath, pathToSave);
        }
    }
}
