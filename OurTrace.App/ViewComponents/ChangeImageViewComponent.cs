using Microsoft.AspNetCore.Mvc;
using OurTrace.App.Models.ViewModels.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OurTrace.App.ViewComponents
{
    public class ChangeImageViewComponent : ViewComponent
    {
        // Prevents unnecessary errors to just keep it async

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<IViewComponentResult> InvokeAsync(bool isGroup, string groupName)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            ChangeImageViewModel viewModel = new ChangeImageViewModel();
            if (isGroup)
            {
                viewModel.IsGroup = isGroup;
                viewModel.GroupName = groupName;
            }
            return View(viewModel);
        }
    }
}
