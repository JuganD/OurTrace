using Microsoft.AspNetCore.Mvc;
using OurTrace.App.Controllers;
using OurTrace.App.Models.ViewModels.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OurTrace.App.ViewComponents
{
    public class PostsViewComponent : ViewComponent
    {
        // Prevents unnecessary errors to just keep it async

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<IViewComponentResult> InvokeAsync(IEnumerable<PostViewModel> model)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            foreach (var postModel in model)
            {
                if (postModel.IsImageOnFileSystem == true)
                    postModel.MediaUrl = $"{this.HttpContext.Request.Scheme}://{this.HttpContext.Request.Host}/File/Image/{postModel.Creator}/{postModel.Id}";
            }
            return View(model);
        }
    }
}
