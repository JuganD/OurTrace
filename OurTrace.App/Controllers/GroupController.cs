using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OurTrace.App.Models.ViewModels.Group;
using OurTrace.Services.Abstraction;

namespace OurTrace.App.Controllers
{
    public class GroupController : Controller
    {
        private readonly IGroupService groupService;

        public GroupController(IGroupService groupService)
        {
            this.groupService = groupService;
        }
        [Authorize]
        public async Task<IActionResult> Discover()
        {
            //await groupService.CreateNewGroupAsync("TestGroup1", "Testcho");

            var viewmodel = await groupService.DiscoverGroupsAsync();
            return View(viewmodel);
        }
        [Authorize]
        public async Task<IActionResult> MyGroups()
        {
            var groups = await groupService.GetUserGroupsAsync(this.User.Identity.Name);
            if (groups != null)
            {
                return View(groups);
            }

            return View();
        }
    }
}