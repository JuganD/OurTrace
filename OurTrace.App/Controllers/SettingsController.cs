using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OurTrace.App.Models.InputModels.Identity.Settings;
using OurTrace.Data.Identity.Models;

namespace OurTrace.App.Controllers
{
    // controller-based authorization
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly SignInManager<OurTraceUser> signInManager;
        private readonly UserManager<OurTraceUser> userManager;

        public SettingsController(SignInManager<OurTraceUser> signInManager,
            UserManager<OurTraceUser> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }
        #region Get

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ChangePassword()
        {
            return RedirectToAction("Index");
        }
        #endregion
        #region Post

        [AutoValidateAntiforgeryToken]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordInputModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound($"Unable to find user. Please contact administrator.");
                }

                var changePasswordResult = await userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (!changePasswordResult.Succeeded)
                {
                    foreach (var error in changePasswordResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View("Settings");
                }

                await signInManager.RefreshSignInAsync(user);
                ViewData["change-password-message"] = "Password successfuly changed!";
                return View("Settings");
            }
            return View("Settings");
        }
        [AutoValidateAntiforgeryToken]
        [HttpPost]
        public async Task<IActionResult> DownloadPersonalData()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to find user. Please contact administrator.");
            }

            // Only include personal data for download
            var personalData = new Dictionary<string, string>();
            var personalDataProps = typeof(OurTraceUser).GetProperties().Where(
                            prop => Attribute.IsDefined(prop, typeof(PersonalDataAttribute)));
            foreach (var p in personalDataProps)
            {
                personalData.Add(p.Name, p.GetValue(user)?.ToString() ?? "null");
            }

            Response.Headers.Add("Content-Disposition", "attachment; filename=PersonalData.json");
            return new FileContentResult(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(personalData)), "text/json");
        }

        [AutoValidateAntiforgeryToken]
        [HttpPost]
        public async Task<IActionResult> AccountDelete(DeleteAccountInputModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound($"Unable to find user. Please contact administrator.");
                }

                await signInManager.SignOutAsync();
                await userManager.DeleteAsync(user);
            }

            return LocalRedirect("/");
        }
        #endregion
    }
}