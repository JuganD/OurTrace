using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OurTrace.App.Models.Identity;
using OurTrace.Data.Identity.Models;
using OurTrace.Services;
using OurTrace.Services.Abstraction;

namespace OurTrace.App.Controllers
{
    public class UserController : Controller
    {
        private readonly SignInManager<OurTraceUser> signInManager;
        private readonly UserManager<OurTraceUser> userManager;
        private readonly IUsersService usersService;

        public UserController(SignInManager<OurTraceUser> signInManager,
            UserManager<OurTraceUser> userManager,
            IUsersService usersService)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.usersService = usersService;
        }

        public async Task<IActionResult> Authenticate()
        {
            // Ensure not signed in
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            return View();
        }
        public IActionResult Login()
        {
            return RedirectToAction("Authenticate");
        }
        public IActionResult Register()
        {
            return RedirectToAction("Authenticate");
        }

        [AutoValidateAntiforgeryToken]
        [HttpPost]
        public async Task<IActionResult> Login(LoginInputModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
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

                var user = this.usersService.GetNewUser(model.Username, model.Email, model.FullName, model.BirthDate);
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
        public IActionResult Lockout()
        {
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return LocalRedirect("/");
        }
    }
}