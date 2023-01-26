using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.Helpers
{
    public interface IFileHelper
    {
        string GetUniqueFileName(string fileName);
        Task<Tuple<string, string>> SaveImageToServerAsync(IFormFile formFile, string[] directory);
        void DeleteImageToServer(string fileName);
    }
}
