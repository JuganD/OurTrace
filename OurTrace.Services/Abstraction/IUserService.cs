using OurTrace.App.Models.ViewModels.Profile;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OurTrace.Services.Abstraction
{
    public interface IUserService
    {
        Task<bool> UserExistsAsync(string username);
        Task<ProfileViewModel> PrepareUserProfileForViewAsync(string actualUserName, string visitingUserName);
        Task<ProfileLastPicturesViewModel> GetLastNPicturesAsync(string username, int picturesCount);
    }
}
