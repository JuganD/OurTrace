using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OurTrace.Data;
using OurTrace.Data.Identity.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OurTrace.Services
{
    internal class RoleService
    {
        private readonly IdentityService identityService;
        private readonly RoleManager<OurTraceRole> roleManager;
        private readonly UserManager<OurTraceUser> userManager;

        internal RoleService(OurTraceDbContext dbContext,
            RoleManager<OurTraceRole> roleManager,
            UserManager<OurTraceUser> userManager)
        {
            this.identityService = new IdentityService(dbContext);
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        public async Task<bool> CreateRoleAsync(string roleName)
        {

            var roleExist = await this.roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                IdentityResult roleResult = await this.roleManager.CreateAsync(new OurTraceRole(roleName));

                return roleResult.Succeeded;
            }
            return false;
        }
        public async Task<bool> AssignRoleAsync(string username, string roleName)
        {
            var roleExist = await this.roleManager.RoleExistsAsync(roleName);
            var user = await this.identityService.GetUserByName(username)
                .SingleOrDefaultAsync();

            if (roleExist && user != null)
            {
                var result = await this.userManager.AddToRoleAsync(user, roleName);
                return result.Succeeded;
            }
            return false;
        }
    }
}
