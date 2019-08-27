using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OurTrace.Services.Abstraction;

namespace OurTrace.App.Controllers
{
    [Authorize]
    [Route("Search/{action}/{*query}")]
    public class SearchController : Controller
    {
        private readonly ISearchService searchService;

        public SearchController(ISearchService searchService)
        {
            this.searchService = searchService;
        }

        [HttpGet]
        public async Task<IActionResult> All(string query)
        {
            if (!string.IsNullOrEmpty(query) && query.Length >= 3)
            {
                var model = await this.searchService.SearchForEverythingAsync(query, this.User.Identity.Name);
                model.Query = query;

                return View(model);
            }
            return View("Error", "Search text length should be at least 3 characters long.");
        }

        [HttpGet]
        public async Task<IActionResult> Users(string query)
        {
            if (!string.IsNullOrEmpty(query) && query.Length >= 3)
            {
                var model = await this.searchService.SearchForUsersAsync(query);
                model.Query = query;

                return View(model);
            }
            return View("Error", "Search text length should be at least 3 characters long.");
        }

        [HttpGet]
        public async Task<IActionResult> Groups(string query)
        {
            if (!string.IsNullOrEmpty(query) && query.Length >= 3)
            {
                var model = await this.searchService.SearchForGroupsAsync(query);
                model.Query = query;

                return View(model);
            }
            return View("Error", "Search text length should be at least 3 characters long.");
        }

        [HttpGet]
        public async Task<IActionResult> Posts(string query)
        {
            if (!string.IsNullOrEmpty(query) && query.Length >= 3)
            {
                var model = await this.searchService.SearchForPostsAsync(query, this.User.Identity.Name);
                model.Query = query;

                return View(model);
            }
            return View("Error", "Search text length should be at least 3 characters long.");
        }

        [HttpGet]
        public async Task<IActionResult> Comments(string query)
        {
            if (!string.IsNullOrEmpty(query) && query.Length >= 3)
            {
                var model = await this.searchService.SearchForCommentsAsync(query, this.User.Identity.Name);
                model.Query = query;

                return View(model);
            }
            return View("Error", "Search text length should be at least 3 characters long.");
        }

    }
}