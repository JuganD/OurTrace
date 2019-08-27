using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OurTrace.Data;
using OurTrace.Data.Identity.Models;
using OurTrace.App.Models;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace OurTrace.App.Controllers
{
    public class HomeController : Controller
    {

        public HomeController()
        {

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
