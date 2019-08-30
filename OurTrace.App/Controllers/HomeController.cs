﻿using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OurTrace.Data;
using OurTrace.Data.Identity.Models;
using OurTrace.App.Models;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.DependencyInjection;
using OurTrace.Services.Abstraction;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using OurTrace.Services.Seeding;

namespace OurTrace.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomeService homeService;
        private readonly ISeeder seeder;

        public HomeController(IHomeService homeService,
            ISeeder seeder)
        {
            this.homeService = homeService;
            this.seeder = seeder;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            await this.seeder.SeedAdverts(50);
            if (this.User.Identity.IsAuthenticated)
            {
                string userId = await this.homeService.GetUserIdFromName(this.User.Identity.Name);
                if (!string.IsNullOrEmpty(userId))
                {
                    var newsFeedModel = await this.homeService.GetNewsfeedViewModelAsync(userId);
                    
                    return View(newsFeedModel);
                }
            }
            return View();
        }
        [Authorize]
        [AutoValidateAntiforgeryToken]
        [HttpPost]
        public async Task<IActionResult> ReceivePosts()
        {
            string userId = await this.homeService.GetUserIdFromName(this.User.Identity.Name);
            if (!string.IsNullOrEmpty(userId))
            {
                var model = await this.homeService.GetNewsfeedViewModelAsync(userId);
                return ViewComponent("Posts", new { model = model.Posts });
            }
            return Unauthorized();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
