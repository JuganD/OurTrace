using GeekLearning.Storage;
using GeekLearning.Storage.Internal;
using Microsoft.AspNetCore.Http;
using OurTrace.Services.Abstraction;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OurTrace.Services
{
    public class FileService : IFileService
    {
        private readonly IStore fileStore;
        private readonly WebClient webClient;
        public FileService(IStorageFactory storageFactory)
        {
            this.fileStore = storageFactory.GetStore("LocalFileStorage");
            this.webClient = new WebClient();
        }
        public async Task SaveImageAsync(IFormFile file, string folder, string fileName)
        {
            using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                var fileBytes = ms.ToArray();
                await fileStore.SaveAsync(fileBytes, new PrivateFileReference(Path.Combine(folder, fileName)), "image/jpeg");
            }
        }
        public async Task SaveImageAsync(string fileUrl, string folder, string fileName)
        {
            await fileStore.SaveAsync(webClient.DownloadData(fileUrl), new PrivateFileReference(Path.Combine(folder, fileName)), "image/jpeg");
        }
    }
}
