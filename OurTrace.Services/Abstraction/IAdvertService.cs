using OurTrace.App.Models.ViewModels.Advert;
using OurTrace.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OurTrace.Services.Abstraction
{
    public interface IAdvertService
    {
        Task<ICollection<AdvertViewModel>> GetRandomAdvertsAndSanctionThemAsync(int count);
        Task AddAdvertAsync(string issuerName, AdvertType type, string content, int viewsLeft);
        Task<ICollection<AdvertViewModel>> GetAllAdvertsAsync();
    }
}
