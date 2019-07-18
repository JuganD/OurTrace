using Microsoft.AspNetCore.Identity;
using OurTrace.App.Models.Authenticate;
using OurTrace.App.Models.ViewModels.Profile;
using OurTrace.Data.Identity.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OurTrace.Services.Abstraction
{
    public interface IIdentityService
    {
        OurTraceUser GetNewUser(RegisterInputModel model);
        Task<List<OurTraceUser>> GetAllUsersAsync();

        Task<OurTraceUser> GetUserAsync(string username);
        Task<OurTraceUser> GetUserByIdAsync(string id);
    }
}
