using OurTrace.App.Models.Advert;
using OurTrace.App.Models.ViewModels.Advert;
using OurTrace.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OurTrace.Services.Abstraction
{
    public interface IAdvertService
    {
        Task<ICollection<AdvertViewModel>> GetRandomAdvertsAndSanctionThemAsync(int count);
        Task<ICollection<AdvertViewModel>> GetAllAdvertsAsync();
        Task<bool> AdvertExistsAsync(string id);
        Task AddAdvertAsync(ModifyAdvertInputModel model);
        Task<bool> ModifyAdvertByIdAsync(ModifyAdvertInputModel model);
    }
}
