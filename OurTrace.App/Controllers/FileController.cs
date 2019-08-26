using System.IO;
using System.Threading.Tasks;
using GeekLearning.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OurTrace.App.Models.InputModels.Global;
using OurTrace.Services.Abstraction;

namespace OurTrace.App.Controllers
{
    // NEEDS AUTHORIZATION
    [Authorize]
    public class FileController : Controller
    {
        private readonly IPostService postService;
        private readonly IFileService fileService;
        private readonly IGroupService groupService;
        private readonly IStore fileStore;

        public FileController(IStorageFactory storageFactory,
            IPostService postService,
            IFileService fileService,
            IGroupService groupService)
        {
            this.fileStore = storageFactory.GetStore("LocalFileStorage");
            this.postService = postService;
            this.fileService = fileService;
            this.groupService = groupService;
        }

        [HttpGet("file/image/{name}/{id}")]
        public async Task<IActionResult> Image(string name, string id)
        {
            var fileRef = await this.fileStore.GetAsync(Path.Combine(name, id));
            if (fileRef != null)
            {
                if (await postService.IsUserCanSeePostAsync(this.User.Identity.Name, id))
                {
                    return File(await fileRef.ReadAllBytesAsync(), "image/jpeg");
                }
            }
            return Unauthorized();
        }

        [HttpGet("file/profilepicture/{name}")]
        public async Task<IActionResult> ProfilePicture(string name)
        {
            var fileRef = await this.fileStore.GetAsync(Path.Combine(name, "FrontPicture"));
            if (fileRef != null)
            {
                return File(await fileRef.ReadAllBytesAsync(), "image/jpeg");
            }
            return File("~/images/default_user.png", "image/png");
        }

        [HttpGet("file/grouppicture/{name}")]
        public async Task<IActionResult> GroupPicture(string name)
        {
            var fileRef = await this.fileStore.GetAsync(Path.Combine("Group_" + name, "FrontPicture"));
            if (fileRef != null)
            {
                return File(await fileRef.ReadAllBytesAsync(), "image/jpeg");
            }
            return File("~/images/default_group.png", "image/png");
        }
        [HttpGet("file/coverpicture/{name}")]
        public async Task<IActionResult> CoverPicture(string name)
        {
            var fileRef = await this.fileStore.GetAsync(Path.Combine(name, "CoverPicture"));
            if (fileRef != null)
            {
                return File(await fileRef.ReadAllBytesAsync(), "image/jpeg");
            }
            return File("~/images/default_background.jpg", "image/png");
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> ChangePicture(ChangeImageInputModel model)
        {
            if (ModelState.IsValid)
            {
                string folderName = this.User.Identity.Name;
                if (model.IsGroup)
                {
                    var groupOwner = await this.groupService.GetGroupOwnerAsync(model.GroupName);
                    if (groupOwner != this.User.Identity.Name)
                    {
                        return Unauthorized();
                    }
                    else
                    {
                        folderName = "Group_" + model.GroupName;
                    }
                }
                if (model.FrontImageMediaFile != null)
                {
                    await this.fileService.SaveImageAsync(model.FrontImageMediaFile, folderName, "FrontPicture");
                }
                if (model.CoverImageMediaFile != null)
                {
                    await this.fileService.SaveImageAsync(model.CoverImageMediaFile, folderName, "CoverPicture");
                }
            }
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}