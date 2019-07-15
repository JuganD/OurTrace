using Microsoft.AspNetCore.Identity;
using OurTrace.Data.Identity.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OurTrace.Services.Abstraction
{
    public interface IUsersService
    {
        OurTraceUser GetNewUser(string username, string email, string fullname, DateTime? birthDate);
        Task<List<OurTraceUser>> GetAllUsersAsync();

        Task<OurTraceUser> GetUserAsync(string username);
        Task<OurTraceUser> GetUserByIdAsync(string id);

        Task AddFollowerAsync(OurTraceUser sender, OurTraceUser recipient);
        Task<bool> CheckFollowExistsAsync(OurTraceUser sender, OurTraceUser recipient);
        Task AddLikeAsync(string postId, string userId);
    }
}
