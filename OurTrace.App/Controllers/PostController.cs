using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OurTrace.App.Models.InputModels.Posts;
using OurTrace.Services.Abstraction;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                Response.Cookies.Append("Errors", errorsAsString,new Microsoft.AspNetCore.Http.CookieOptions()
                {
                    IsEssential = true
                });
            }

            return Redirect(referer);
        }
    }
}