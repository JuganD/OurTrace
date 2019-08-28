using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OurTrace.App.Models.Advert;
using OurTrace.Data.Identity.Models;
using OurTrace.Services.Abstraction;

namespace OurTrace.App.Areas.Administration.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Administration")]
    public class AdvertsController : Controller
    {
        private readonly IAdvertService advertService;

        public AdvertsController(IAdvertService advertService)
        {
            this.advertService = advertService;
        }
        public async Task<IActionResult> Modify()
        {
            var adverts = await this.advertService.GetAllAdvertsAsync();
            return View(adverts);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Modify(ModifyAdvertInputModel model)
        {
            if (ModelState.IsValid)
            {
                if (await this.advertService.AdvertExistsAsync(model.Id) || model.Id != null)
                {
                    await this.advertService.ModifyAdvertByIdAsync(model);
                }
                else
                {
                    await this.advertService.AddAdvertAsync(model);
                }
            }
            return RedirectToAction("Modify");
        }
    }
}