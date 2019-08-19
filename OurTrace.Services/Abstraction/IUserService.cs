using OurTrace.App.Models.ViewModels.Profile;
using System.Threading.Tasks;

namespace OurTrace.Services.Abstraction
{
    public interface IUserService
    {
        Task<ProfileViewModel> PrepareUserProfileForViewAsync(string actualUserName, string visitingUserName);
    }
}
