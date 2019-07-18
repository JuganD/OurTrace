using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OurTrace.App.Models.InputModels.Posts;
using OurTrace.Services.Abstraction;
using System.Threading.Tasks;

namespace OurTrace.App.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostService postService;

        public PostController(IPostService postService)
        {
            this.postService = postService;
        }
        [Authorize]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(CreatePostInputModel model)
        {
            await postService.CreateNewPostAsync(this.User.Identity.Name, model);

            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}