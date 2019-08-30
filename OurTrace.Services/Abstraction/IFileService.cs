using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OurTrace.Services.Abstraction
{
    public interface IFileService
    {
        Task SaveImageAsync(IFormFile file, string folder, string fileName);
        Task SaveImageAsync(string fileUrl, string folder, string fileName);
    }
}
