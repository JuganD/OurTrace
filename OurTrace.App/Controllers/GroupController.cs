using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OurTrace.App.Models.InputModels.Group;
using OurTrace.Services.Abstraction;

namespace OurTrace.App.Controllers
{
    // NEEDS AUTHORIZATION
    [Authorize]
    public class GroupController : Controller
    {
        private readonly IGroupService groupService;

        public GroupController(IGroupService groupService)
        {
            this.groupService = groupService;
        }
        public async Task<IActionResult> Discover()
        {
            var viewmodel = await groupService.DiscoverGroupsAsync(this.User.Identity.Name);
            return View(viewmodel);
        }
        public async Task<IActionResult> MyGroups()
        {
            var groups = await groupService.GetUserGroupsAsync(this.User.Identity.Name);
            if (groups != null)
            {
                return View(groups);
            }

            return View();
        }

        public async Task<IActionResult> Open(string name)
        {
            var group = await this.groupService.PrepareGroupForViewAsync(name, this.User.Identity.Name);

            if (group != null)
            {
                return View(group);
            }

            return RedirectToAction("MyGroups");
        }

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
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AcceptMember(string groupname, string membername)
        {
            bool canInvokeAcceptance =
               await this.groupService.IsUserHaveAnyAdministratorRightsAsync(groupname, this.User.Identity.Name);

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
        public async Task<IActionResult> ViewAllMembers(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return RedirectToAction("MyGroups");
            }

            bool userParticipatesInGroup = await this.groupService
                .IsUserConfirmedMemberAsync(name, this.User.Identity.Name);

            if (userParticipatesInGroup)
            {
                var members = await this.groupService
                    .GetGroupMembersAsync(name);

                bool isCurrentUserAdministrator =
                    await this.groupService.IsUserHaveAnyAdministratorRightsAsync(name, this.User.Identity.Name);

                ViewData.Add("IsUserAdministrator", isCurrentUserAdministrator);

                return View(members);
            }

            return RedirectToAction("MyGroups");
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> KickMember(string group, string username)
        {
            if (string.IsNullOrEmpty(group) || string.IsNullOrEmpty(username))
            {
                return RedirectToAction("ViewAllMembers", new { name = group });
            }

            bool issuerParticipatesInGroup = await this.groupService
                .IsUserConfirmedMemberAsync(group, this.User.Identity.Name);

            bool kickedUserParticipatesInGroup = await this.groupService
                .IsUserConfirmedMemberAsync(group, username);

            if (issuerParticipatesInGroup && kickedUserParticipatesInGroup)
            {
                bool isIssuerUserAdministrator =
                    await this.groupService.IsUserHaveAnyAdministratorRightsAsync(group, this.User.Identity.Name);

                // Because admins should be able to kick every other role
                bool isKickedUserAdministrator =
                    await this.groupService.IsUserHaveRoleAsync(group, username, "Admin");

                if (isIssuerUserAdministrator == true && isKickedUserAdministrator == false)
                {
                    if (await this.groupService.KickMemberAsync(group, username))
                    {
                        TempData["KickResult"] = "Successful!";
                    }
                    else
                    {
                        TempData["KickResult"] = "Failed!";
                    }
                }
                else
                {
                    TempData["KickResult"] = "Not enough privileges to kick this user.";
                }
            }
            else
            {
                TempData["KickResult"] = "Invalid data.";
            }

            return RedirectToAction("ViewAllMembers", new { name = group });
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Leave(string groupname)
        {
            await this.groupService.KickMemberAsync(groupname, this.User.Identity.Name);
            return RedirectToAction("Discover");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(GroupCreateInputModel model)
        {
            if (ModelState.IsValid)
            {
                if (!await this.groupService.CreateNewGroupAsync(model.Name, this.User.Identity.Name))
                {
                    TempData.Add("CreateGroupResult", "Failed to create group! Group name already taken!");
                } else
                {
                    return RedirectToAction("MyGroups");
                }
            }
            return View();
        }
    }
}