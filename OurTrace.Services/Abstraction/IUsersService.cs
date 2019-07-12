using Microsoft.AspNetCore.Identity;
using OurTrace.Data.Identity.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OurTrace.Services.Abstraction
{
    public interface IUsersService
    {
        List<OurTraceUser> GetAllUsers();

        OurTraceUser GetUser(string username);

        OurTraceUser GetNewUser(string username, string email, string fullname, DateTime? birthDate);
    }
}
