using Microsoft.AspNetCore.Http;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

namespace CRM.Core.Business.Helpers
{
    public interface IFileHelper
    {
        string GetUniqueFileName(string fileName);
        Task<Tuple<string, string>> SaveImageToServerAsync(IFormFile formFile, string[] directory);
        void DeleteImageToServer(string fileName);
        List<Return> ReadCsvFile<Return, Mapper>(IFormFile file) where Return: IFileReadable where Mapper: ClassMap<Return>;
    }
}
