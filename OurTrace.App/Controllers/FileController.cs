using System.IO;
using System.Threading.Tasks;
using GeekLearning.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OurTrace.Services.Abstraction;

namespace OurTrace.App.Controllers
{
    [Authorize]
    public class FileController : Controller
    {
        private readonly IPostService postService;
        private readonly IStore fileStore;

        public FileController(IStorageFactory storageFactory,
            IPostService postService)
        {
            this.fileStore = storageFactory.GetStore("LocalFileStorage");
            this.postService = postService;
        }

        [HttpGet("file/image/{username}/{id}")]
        public async Task<IActionResult> Image(string username, string id)
        {
            var fileRef = await this.fileStore.GetAsync(Path.Combine(username, id));
            if (fileRef != null)
            {
                if (await postService.IsUserCanSeePostAsync(this.User.Identity.Name, id))
                {
                    return File(await fileRef.ReadAllBytesAsync(), "image/jpeg");
                }
            }
            return Unauthorized();
        }
    }
}