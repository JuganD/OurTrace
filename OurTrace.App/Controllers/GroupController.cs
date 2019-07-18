using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace OurTrace.App.Controllers
{
    public class GroupController : Controller
    {
        public async Task<IActionResult> Discover()
        {
            return View();
        }
    }
}