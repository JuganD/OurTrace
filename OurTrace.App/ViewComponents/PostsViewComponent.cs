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
        public async Task<IViewComponentResult> InvokeAsync(IEnumerable<PostViewModel> model)
        {
            return View(model);
        }
    }
}
