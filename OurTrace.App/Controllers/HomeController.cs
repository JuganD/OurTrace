﻿using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OurTrace.Data;
using OurTrace.Data.Identity.Models;
using OurTrace.App.Models;

namespace OurTrace.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly SignInManager<OurTraceUser> _signInManager;
        private readonly OurTraceDbContext dbContext;
        private readonly UserManager<OurTraceUser> _userManager;

        public HomeController(
            UserManager<OurTraceUser> userManager,
            SignInManager<OurTraceUser> signInManager,
            OurTraceDbContext dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this.dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return View();
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
