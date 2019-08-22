using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OurTrace.App.Models.InputModels.Settings;
using OurTrace.App.Models.ViewModels.Settings;
using OurTrace.Data.Identity.Models;
using OurTrace.Services.Abstraction;

namespace OurTrace.App.Areas.Settings.Controllers
{
    // NEEDS AUTHORIZATION
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly SignInManager<OurTraceUser> signInManager;
        private readonly UserManager<OurTraceUser> userManager;
        private readonly IRelationsService relationsService;

        public SettingsController(SignInManager<OurTraceUser> signInManager,
            UserManager<OurTraceUser> userManager,
            IRelationsService relationsService)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.relationsService = relationsService;
        }
        #region Get
        public async Task<IActionResult> Index()
        {
            return View(await GetIndexViewModel());
        }
        public IActionResult ChangePassword()
        {
            return RedirectToAction("Index");
        }
        #endregion
        #region Post

        [HttpPost]
        [AutoValidateAntiforgeryToken]
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
                    return View("Index", await GetIndexViewModel());
                }

                await signInManager.RefreshSignInAsync(user);
                ViewData["change-password-message"] = "Password successfuly changed!";
                return View("Index", await GetIndexViewModel());
            }
            return View("Index", await GetIndexViewModel());
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
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

        [HttpPost]
        [AutoValidateAntiforgeryToken]
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
        [NonAction]
        private async Task<ICollection<SettingsFriendRequestViewModel>> GetIndexViewModel()
        {
            return await relationsService
                .GetPendingFriendRequestsAsync(this.User.Identity.Name);
        }
    }
}