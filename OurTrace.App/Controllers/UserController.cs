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
        private readonly IRelationsService relationsService;
        private readonly IMapper mapper;

        public UserController(IRelationsService relationsService,
            IMapper mapper)
        {
            this.relationsService = relationsService;
            this.mapper = mapper;
        }
        [Authorize]
        public async Task<IActionResult> Profile(string username)
        {
            if (string.IsNullOrEmpty(username))
                return await Profile(this.User.Identity.Name);

            if (!string.IsNullOrEmpty(username))
            {
                var profileViewModel = await relationsService.PrepareUserProfileForViewAsync(
                    this.User.Identity.Name, username);

                return View(profileViewModel);
            }

            return LocalRedirect("/");
        }
    }
}