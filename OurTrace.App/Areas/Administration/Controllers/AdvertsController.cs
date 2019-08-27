using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OurTrace.App.Areas.Administration.Models.InputModels;
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
        public async Task<IActionResult> All()
        {
            var adverts = await this.advertService.GetAllAdvertsAsync();
            return View(adverts);
        }
        public IActionResult Add()
        {
            return View();
        }
        public async Task<IActionResult> Add(AddAdvertInputModel model)
        {
            if (ModelState.IsValid)
            {
                await this.advertService.AddAdvertAsync(model.IssuerName, model.Type, model.Content, model.ViewsLeft);
            }
            return RedirectToAction("All");
        }
    }
}