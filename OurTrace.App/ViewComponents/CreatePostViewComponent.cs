using Microsoft.AspNetCore.Mvc;
using OurTrace.App.Models.InputModels.Posts;
using OurTrace.App.Models.ViewModels.Post;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OurTrace.App.ViewComponents
{
    public class CreatePostViewComponent : ViewComponent
    {
        // Prevents unnecessary errors to just keep it async

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<IViewComponentResult> InvokeAsync(string model)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            return View(new CreatePostInputModel() { Location = model });
        }
    }
}
