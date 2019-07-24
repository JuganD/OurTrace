using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OurTrace.App.Models.Authenticate;
using OurTrace.App.Models.ViewModels.Profile;
using OurTrace.Data;
using OurTrace.Data.Identity.Models;
using OurTrace.Data.Models;
using OurTrace.Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OurTrace.Services
{
    // I decided that it would be an excelent performance step-up, if each and every service
    // includes only what it needs. So, this service is internal for all services
    // and its main purpose is to expose IQueriable results to other services
    // and they decide what to include. This way we don't include things we don't use and save performance.
    internal class IdentityService
    {
        private readonly OurTraceDbContext dbContext;

        internal IdentityService(OurTraceDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        
        // Day 7941: Still no use for this method.
        public async Task<List<OurTraceUser>> GetAllUsersAsync()
        {
            return await dbContext.Users.ToListAsync();
        }

        public IQueryable<OurTraceUser> GetUserByName(string username)
        {
            return this.dbContext.Users.Where(x => x.UserName == username);
        }
        public IQueryable<OurTraceUser> GetUserById(string id)
        {
            return this.dbContext.Users.Where(x => x.Id == id);
        }
    }
}
