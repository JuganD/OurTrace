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

        [HttpGet("file/profilepicture/{username}")]
        public async Task<IActionResult> ProfilePicture(string username)
        {
            var fileRef = await this.fileStore.GetAsync(Path.Combine(username, "ProfilePicture"));
            if (fileRef != null)
            {
                return File(await fileRef.ReadAllBytesAsync(), "image/jpeg");
            }
            return File("~/images/default_user.png", "image/png");
        }

        [HttpGet("file/grouppicture/{username}")]
        public async Task<IActionResult> GroupPicture(string name)
        {
            var fileRef = await this.fileStore.GetAsync(Path.Combine("Group_"+name, "GroupPicture"));
            if (fileRef != null)
            {
                return File(await fileRef.ReadAllBytesAsync(), "image/jpeg");
            }
            return File("~/images/default_group.png", "image/png");
        }
    }
}