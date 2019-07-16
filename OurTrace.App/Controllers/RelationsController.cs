﻿using Microsoft.AspNetCore.Mvc;
using OurTrace.Services.Abstraction;
using System.Threading.Tasks;

namespace OurTrace.App.Controllers
{
    public class RelationsController : Controller
    {
        private readonly IIdentityService identityService;
        private readonly IRelationsService relationsService;

        public RelationsController(IIdentityService usersService, 
            IRelationsService relationsService)
        {
            this.identityService = usersService;
            this.relationsService = relationsService;
        }

        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> SendFriendship(string receiver)
        {
            if (!string.IsNullOrEmpty(this.User.Identity.Name) &&
                !string.IsNullOrEmpty(receiver))
            {
                await relationsService.AddFriendshipAsync(this.User.Identity.Name, receiver);
            }

            // Returns user to the same page
            // Safe cause if its from foreign page, AntiforgeryToken would 
            // forbid the controller to get here
            return Redirect(Request.Headers["Referer"].ToString());
        }

        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> RevokeFriendship(string receiver)
        {
            if (!string.IsNullOrEmpty(this.User.Identity.Name) &&
                !string.IsNullOrEmpty(receiver))
            {
                await relationsService.RevokeFriendshipAsync(this.User.Identity.Name, receiver);
            }

            // Returns user to the same page
            // Safe cause if its from foreign page, AntiforgeryToken would 
            // forbid the controller to get here
            return Redirect(Request.Headers["Referer"].ToString());
        }

        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Follow(string receiver)
        {
            if (!string.IsNullOrEmpty(this.User.Identity.Name) &&
                !string.IsNullOrEmpty(receiver))
            {
                await relationsService.AddFollowerAsync(this.User.Identity.Name, receiver);
            }

            // Returns user to the same page
            // Safe cause if its from foreign page, AntiforgeryToken would 
            // forbid the controller to get here
            return Redirect(Request.Headers["Referer"].ToString());
        }

        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Unfollow(string receiver)
        {
            if (!string.IsNullOrEmpty(this.User.Identity.Name) &&
                !string.IsNullOrEmpty(receiver))
            {
                await relationsService.RevokeFollowingAsync(this.User.Identity.Name, receiver);
            }

            // Returns user to the same page
            // Safe cause if its from foreign page, AntiforgeryToken would 
            // forbid the controller to get here
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}