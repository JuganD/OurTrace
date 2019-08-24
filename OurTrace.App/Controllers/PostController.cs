using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OurTrace.App.Models.InputModels.Posts;
using OurTrace.Services.Abstraction;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OurTrace.App.Controllers
{
    // NEEDS AUTHORIZATION
    [Authorize]
    public class PostController : Controller
    {
        private readonly IPostService postService;

        public PostController(IPostService postService)
        {
            this.postService = postService;
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(CreatePostInputModel model)
        {
            var referer = Request.Headers["Referer"].ToString();
            if (ModelState.IsValid)
            {
                await postService.CreateNewPostAsync(this.User.Identity.Name, model);
            }
            else
            {
                // Yea it may not be the best way to pass errors, but sure as hell works fine
                List<string> errors = ModelState.Values.SelectMany(x => x.Errors.Select(y => y.ErrorMessage)).ToList();
                string errorsAsString = string.Join(";", errors);
                Response.Cookies.Append("Errors", errorsAsString, new Microsoft.AspNetCore.Http.CookieOptions()
                {
                    IsEssential = true
                });
            }

            return Redirect(referer);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Like(string postId)
        {
            if (await postService.IsUserCanSeePostAsync(this.User.Identity.Name, postId))
            {
                if (await this.postService.LikePostAsync(this.User.Identity.Name, postId))
                    return StatusCode(200, "Ok");
            }

            return StatusCode(403, "Forbidden");
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> LikeComment(string postId, string commentId)
        {
            if (await postService.IsUserCanSeePostAsync(this.User.Identity.Name, postId))
            {
                if (await this.postService.LikeCommentAsync(this.User.Identity.Name, commentId))
                    return StatusCode(200, "Ok");
            }

            return StatusCode(403, "Forbidden");
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Comment(string postId, string content)
        {
            if (await postService.IsUserCanSeePostAsync(this.User.Identity.Name, postId))
            {
                if (await this.postService.CommentPostAsync(this.User.Identity.Name, postId, content))
                    return StatusCode(200, "Ok");
            }

            return StatusCode(403, "Forbidden");
        }
    }
}