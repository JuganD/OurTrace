using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OurTrace.Data.Identity.Models;
using OurTrace.Services.Abstraction;

namespace OurTrace.App.Controllers
{
    // NEEDS AUTHORIZATION
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }
        public async Task<IActionResult> Profile(string username)
        {
            if (string.IsNullOrEmpty(username))
                return await Profile(this.User.Identity.Name);

            if (!string.IsNullOrEmpty(username))
            {
                var profileViewModel = await userService.PrepareUserProfileForViewAsync(
                    this.User.Identity.Name, username);

                if (profileViewModel != null)
                    return View(profileViewModel);
            }

            return LocalRedirect("/");
        }
    }
}