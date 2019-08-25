using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OurTrace.App.Models.InputModels.Posts;
using OurTrace.App.Models.InputModels.Share;
using OurTrace.App.Models.ViewModels.Post;
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
        private readonly INotificationService notificationService;

        public PostController(IPostService postService,
            INotificationService notificationService)
        {
            this.postService = postService;
            this.notificationService = notificationService;
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
                {
                    var usernameOfPostOwner = await postService.GetPostOwnerUsernameAsync(postId);
                    if (this.User.Identity.Name != usernameOfPostOwner)
                    {
                        await this.notificationService.AddNotificationToUserAsync(new Services.Models.NotificationServiceModel()
                        {
                            Action = "Open",
                            Controller = "Post",
                            ElementId = postId,
                            Username = usernameOfPostOwner,
                            Content = this.User.Identity.Name + " has liked your post!"
                        });
                    }
                    return StatusCode(200, "Ok");
                }
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
                {
                    var usernameOfCommentOwner = await postService.GetCommentOwnerUsernameAsync(commentId);
                    if (this.User.Identity.Name != usernameOfCommentOwner)
                    {
                        await this.notificationService.AddNotificationToUserAsync(new Services.Models.NotificationServiceModel()
                        {
                            Action = "Open",
                            Controller = "Post",
                            ElementId = postId,
                            Username = usernameOfCommentOwner,
                            Content = this.User.Identity.Name + " has liked your comment!"
                        });
                    }
                    return StatusCode(200, "Ok");
                }
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
                {
                    var usernameOfPostOwner = await postService.GetPostOwnerUsernameAsync(postId);
                    if (this.User.Identity.Name != usernameOfPostOwner)
                    {
                        await this.notificationService.AddNotificationToUserAsync(new Services.Models.NotificationServiceModel()
                        {
                            Action = "Open",
                            Controller = "Post",
                            ElementId = postId,
                            Username = usernameOfPostOwner,
                            Content = this.User.Identity.Name + " has commented your post!"
                        });
                    }

                    return StatusCode(200, "Ok");
                }
            }

            return StatusCode(403, "Forbidden");
        }
        [HttpGet("/Post/Share/{postId}")]
        public async Task<IActionResult> Share(string postId)
        {
            var viewModel = await postService.GetShareViewAsync(postId);
            return View(viewModel);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Share(ShareInputModel model)
        {
            if (ModelState.IsValid)
            {
                if (await this.postService.SharePostAsync(this.User.Identity.Name, model))
                {
                    if (model.ShareLocationType == ShareLocation.FriendWall)
                    {
                        return RedirectToAction("Profile", "User", new { username = model.ShareLocation });
                    }
                    if (model.ShareLocationType == ShareLocation.GroupWall)
                    {
                        return RedirectToAction("Open", "Group", new { name = model.ShareLocation });
                    }
                    return RedirectToAction("Profile", "User", new { username = this.User.Identity.Name });
                }
            }

            var viewModel = await postService.GetShareViewAsync(model.PostId);
            TempData["ShareResult"] = "Failed! Please check where are you trying to share the post: did you spelled the name correctly?, do you have rights to post there?";
            return View(viewModel);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete(string postId)
        {
            if (await postService.DeletePostAsync(this.User.Identity.Name, postId))
            {
                return StatusCode(200, "Ok");
            }
            return StatusCode(403, "Forbidden");
        }
        [HttpGet("/Post/Open/{Id}")]
        public async Task<IActionResult> Open(string id)
        {
            var postmodel = await this.postService.GetPostViewAsync(id);
            if (postmodel != null)
            {
                return View(new List<PostViewModel>() { postmodel });
            }
            return NotFound();
        }
    }
}