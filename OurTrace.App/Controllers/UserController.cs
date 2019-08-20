using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OurTrace.App.Models.ViewModels.Profile;
using OurTrace.Services.Abstraction;

namespace OurTrace.App.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;

        public UserController(IUserService userService,
            IMapper mapper)
        {
            this.userService = userService;
            this.mapper = mapper;
        }
        [Authorize]
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