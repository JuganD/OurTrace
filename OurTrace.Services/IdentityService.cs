using Microsoft.EntityFrameworkCore;
using OurTrace.App.Models.ViewModels.Identity.Profile;
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
    public class IdentityService : IIdentityService
    {
        private readonly OurTraceDbContext dbContext;

        public IdentityService(OurTraceDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public OurTraceUser GetNewUser(string username, string email, string fullname,
            DateTime? birthDate, string country, UserSex sex)
        {
            var wall = new Wall();

            var user = new OurTraceUser
            {
                UserName = username,
                Email = email,
                FullName = fullname,
                BirthDate = birthDate,
                Country = country,
                Sex = sex,
                Wall = wall
            };

            return user;
        }

        public async Task<List<OurTraceUser>> GetAllUsersAsync()
        {
            return await dbContext.Users.ToListAsync();
        }

        public async Task<OurTraceUser> GetUserAsync(string username)
        {
            return await AttachRequiredInclusionsToUser(this.dbContext.Users)
                .SingleOrDefaultAsync(x => x.UserName == username);
        }
        public async Task<OurTraceUser> GetUserByIdAsync(string id)
        {
            return await AttachRequiredInclusionsToUser(this.dbContext.Users)
                .SingleOrDefaultAsync(x => x.Id == id);
        }


        private IQueryable<OurTraceUser> AttachRequiredInclusionsToUser(IQueryable<OurTraceUser> query)
        {
            return query
                .Include(x => x.Followers)
                .Include(x => x.Following)
                .Include(x => x.Wall)
                    .ThenInclude(x => x.Posts)
                .Include(x => x.SentFriendships)
                    .ThenInclude(x => x.Sender)
                 .Include(x => x.SentFriendships)
                    .ThenInclude(x => x.Recipient)
                .Include(x => x.ReceivedFriendships)
                    .ThenInclude(x => x.Sender)
                .Include(x => x.ReceivedFriendships)
                    .ThenInclude(x => x.Recipient)
                .Include(x => x.Comments);
        }
    }
}
