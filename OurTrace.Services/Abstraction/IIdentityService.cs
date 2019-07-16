using Microsoft.AspNetCore.Identity;
using OurTrace.App.Models.ViewModels.Identity.Profile;
using OurTrace.Data.Identity.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OurTrace.Services.Abstraction
{
    public interface IIdentityService
    {
        OurTraceUser GetNewUser(string username, string email, string fullname, DateTime? birthDate, string country, UserSex sex);
        Task<List<OurTraceUser>> GetAllUsersAsync();

        Task<OurTraceUser> GetUserAsync(string username);
        Task<OurTraceUser> GetUserByIdAsync(string id);
    }
}
