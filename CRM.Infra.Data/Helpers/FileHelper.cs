using CRM.Core.Business;
using CRM.Core.Business.Helpers;
using CRM.Core.Domain.Exceptions;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Globalization;

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
            var isDefault = fileName.Contains("default\\default");
            if (isDefault) return;

            var fullPath = Path.Combine(_environment.WebRootPath, fileName);
            if(File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }

        public string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName).Replace(' ', '_');

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

        public List<Return> ReadCsvFile<Return, Mapper>(IFormFile file)
            where Return : IFileReadable
            where Mapper : ClassMap<Return>
        {
            var fileextension = Path.GetExtension(file.FileName);
            if (fileextension != ".csv") throw new BadHttpRequestException($"Please send a csv file not a {fileextension} file.");

            using var stream = file.OpenReadStream();
            using var streamReader = new StreamReader(stream);
            CsvConfiguration csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                HeaderValidated = null,
                MissingFieldFound = null,
                IgnoreBlankLines = true,
                Delimiter= ";",
            };

            using var csvReader = new CsvReader(streamReader, csvConfig);
            csvReader.Context.RegisterClassMap<Mapper>();

            try
            {

                var items = csvReader.GetRecords<Return>().ToList();
                var itemsValidate = GetValidateList(items);

                return itemsValidate;

            } catch(Exception ex)
            {
                throw new InvalidDataException(ex.Message);
            }
        }

        private static List<T> GetValidateList<T>(List<T> elements) where T : IFileReadable
        {
            foreach(var element in elements)
            {
                try
                {
                    ValidatorBehavior<T>.Validate(element);
                } catch(BaseException ex)
                {
                    element.Errors = ex.Errors;
                    element.Status = FIleReadStatus.Invalid;
                }
            }
            return elements.ToList();
        }
    }
}
