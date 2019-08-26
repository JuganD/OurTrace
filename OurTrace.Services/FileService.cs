using GeekLearning.Storage;
using GeekLearning.Storage.Internal;
using Microsoft.AspNetCore.Http;
using OurTrace.Services.Abstraction;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace OurTrace.Services
{
    public class FileService : IFileService
    {
        private readonly IStore fileStore;
        public FileService(IStorageFactory storageFactory)
        {
            this.fileStore = storageFactory.GetStore("LocalFileStorage");
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
    }
}
