using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OurTrace.App.Models.ViewModels.Group;
using OurTrace.App.Models.ViewModels.Post;
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

            var viewmodel = await groupService.DiscoverGroupsAsync(this.User.Identity.Name);
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

        [Authorize]
        public async Task<IActionResult> Open(string name)
        {
            var group = await this.groupService.PrepareGroupForViewAsync(name, this.User.Identity.Name);

            if (group != null)
            {
                return View(group);
            }

            return RedirectToAction("Discover");
        }

        [Authorize]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Join(string name)
        {
            bool canJoinGroup =
                await this.groupService.GroupExistAsync(name) &&
               !await this.groupService.IsUserMemberOfGroupAsync(name, this.User.Identity.Name);

            if (canJoinGroup)
            {
                if (await this.groupService.JoinGroupAsync(name, this.User.Identity.Name))
                {
                    return RedirectToAction("Open", new { name = name });
                }
            }

            return RedirectToAction("Discover");
        }

        // Serves ajax calls
        [Authorize]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AcceptMember(string groupname, string membername)
        {
            bool canInvokeAcceptance =
               await this.groupService.IsUserHaveRoleAsync(groupname, this.User.Identity.Name, "Admin") ||
               await this.groupService.IsUserHaveRoleAsync(groupname, this.User.Identity.Name, "Moderator");

            if (canInvokeAcceptance)
            {
                if (await this.groupService.AcceptMemberAsync(groupname, membername))
                {
                    return Json("Ok");
                }
                else
                {
                    return Json("NotFound");
                }
            }

            return Json("Unauthorized");
        }
        [Authorize]
        public async Task<IActionResult> ViewAllMembers(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return RedirectToAction("Discover");
            }

            bool userParticipatesInGroup = await this.groupService
                .IsUserConfirmedMemberAsync(name, this.User.Identity.Name);

            if (userParticipatesInGroup)
            {
                var members = await this.groupService
                    .GetGroupMembersAsync(name);

                return View(members);
            }

            return RedirectToAction("Discover");
        }
    }
}