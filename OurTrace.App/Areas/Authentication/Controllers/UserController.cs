using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OurTrace.App.Models.Authenticate;
using OurTrace.Data.Identity.Models;
using OurTrace.Services.Abstraction;

namespace OurTrace.App.Areas.Authentication.Controllers
{
    [Area("Authentication")]
    public class UserController : Controller
    {
        private readonly SignInManager<OurTraceUser> signInManager;
        private readonly UserManager<OurTraceUser> userManager;
        private readonly IIdentityService identityService;

        public UserController(SignInManager<OurTraceUser> signInManager,
            UserManager<OurTraceUser> userManager,
            IIdentityService usersService)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.identityService = usersService;
        }

        #region Get
        public IActionResult Index(string ReturnUrl = null)
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
            return RedirectToAction("Index", ReturnUrl);
        }
        public IActionResult Register(string ReturnUrl = null)
        {
            return RedirectToAction("Index");
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
                return View("Index");
            }

            ModelState.AddModelError(string.Empty, "Invalid data provided.");
            return View("Index");
        }

        [AutoValidateAntiforgeryToken]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterInputModel model)
        {
            if (ModelState.IsValid)
            {
                // Creating it here because we are using built in usermanager
                var user = this.identityService.GetNewUser(model);
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
            return View("Index");
        }

        #endregion
    }
}