using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using OurTrace.App.Models.InputModels.Identity;
using OurTrace.App.Models.InputModels.Identity.Settings;
using OurTrace.App.Models.ViewModels.Identity.Profile;
using OurTrace.Data.Identity.Models;
using OurTrace.Services;
using OurTrace.Services.Abstraction;

namespace OurTrace.App.Controllers
{
    public class UserController : Controller
    {
        private readonly SignInManager<OurTraceUser> signInManager;
        private readonly UserManager<OurTraceUser> userManager;
        private readonly IIdentityService identityService;
        private readonly IRelationsService relationsService;
        private readonly IMapper mapper;

        public UserController(SignInManager<OurTraceUser> signInManager,
            UserManager<OurTraceUser> userManager,
            IIdentityService usersService,
            IRelationsService relationsService,
            IMapper mapper)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.identityService = usersService;
            this.relationsService = relationsService;
            this.mapper = mapper;
        }

        #region Get
        public IActionResult Authenticate(string ReturnUrl = null)
        {
            if (this.signInManager.IsSignedIn(this.User))
            {
                return LocalRedirect("/");
            }

            ViewData["ReturnUrl"] = ReturnUrl;
            return View();
        }
        public IActionResult Login(string ReturnUrl = null)
        {
            return RedirectToAction("Authenticate", ReturnUrl);
        }
        public IActionResult Register(string ReturnUrl = null)
        {
            return RedirectToAction("Authenticate");
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

        public IActionResult Lockout()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return LocalRedirect("/");
        }

        #endregion

        #region Post
        [AutoValidateAntiforgeryToken]
        [HttpPost]
        public async Task<IActionResult> Login(LoginInputModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl))
                    {
                        return LocalRedirect(model.ReturnUrl);
                    }
                    return LocalRedirect("/");
                }
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, "This account has been locked out. Try again later.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
                return View("Authenticate");
            }

            ModelState.AddModelError(string.Empty, "Invalid data provided.");
            return View("Authenticate");
        }

        [AutoValidateAntiforgeryToken]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterInputModel model)
        {
            if (ModelState.IsValid)
            {
                // Creating it here because we are using built in usermanager
                var user = this.identityService.GetNewUser(model.Username, model.Email, model.FullName, model.BirthDate, model.Country, model.Sex);
                IdentityResult result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect("/");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            ViewData["auth_location_register"] = true;
            return View("Authenticate");
        }

        #endregion
    }
}