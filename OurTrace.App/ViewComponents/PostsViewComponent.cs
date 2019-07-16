using Microsoft.AspNetCore.Mvc;
using OurTrace.App.Models.ViewModels.Posts;
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
            return View(model);
        }
    }
}
