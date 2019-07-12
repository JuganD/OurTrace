using Microsoft.AspNetCore.Identity;
using OurTrace.Data;
using OurTrace.Data.Identity.Models;
using OurTrace.Data.Models;
using OurTrace.Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OurTrace.Services
{
    public class UsersService : IUsersService
    {
        private readonly OurTraceDbContext dbContext;

        public UsersService(OurTraceDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public OurTraceUser GetNewUser(string username, string email, string fullname, DateTime? birthDate)
        {
            var wall = new Wall();

            var user = new OurTraceUser
            {
                UserName = username,
                Email = email,
                FullName = fullname,
                BirthDate = birthDate,
                Wall = wall
            };

            return user;
        }

        public List<OurTraceUser> GetAllUsers()
        {
            return dbContext.Users.ToList();
        }

        public OurTraceUser GetUser(string username)
        {
            return dbContext.Users.SingleOrDefault(x => x.UserName == username);
        }
    }
}
