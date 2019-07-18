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
        private readonly IIdentityService identityService;
        private readonly IRelationsService relationsService;
        private readonly IMapper mapper;

        public UserController(IIdentityService usersService,
            IRelationsService relationsService,
            IMapper mapper)
        {
            this.identityService = usersService;
            this.relationsService = relationsService;
            this.mapper = mapper;
        }
        [Authorize]
        public async Task<IActionResult> Profile(string username)
        {
            if (string.IsNullOrEmpty(username))
                return await Profile(this.User.Identity.Name);

            var visitingUser = await identityService.GetUserAsync(username);
            if (visitingUser != null)
            {
                var profileInfo = mapper.Map<ProfileViewModel>(visitingUser);
                
                await relationsService.PrepareUserProfileForViewAsync(profileInfo,
                    this.User.Identity.Name, visitingUser);

                return View(profileInfo);
            }
            return LocalRedirect("/");
        }
    }
}