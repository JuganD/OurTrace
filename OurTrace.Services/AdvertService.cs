using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OurTrace.App.Models.ViewModels.Advert;
using OurTrace.Data;
using OurTrace.Data.Models;
using OurTrace.Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OurTrace.Services
{
    public class AdvertService : IAdvertService
    {
        private readonly OurTraceDbContext dbContext;
        private readonly IMapper automapper;

        public AdvertService(OurTraceDbContext dbContext,
            IMapper automapper)
        {
            this.dbContext = dbContext;
            this.automapper = automapper;
        }

        public async Task AddAdvertAsync(string issuerName, AdvertType type, string content, int viewsLeft)
        {
            if (!string.IsNullOrEmpty(issuerName) && viewsLeft > 0)
            {
                await this.dbContext.Adverts.AddAsync(new Advert()
                {
                    IssuerName = issuerName,
                    Type = type,
                    Content = content,
                    ViewsLeft = viewsLeft
                });
                await this.dbContext.SaveChangesAsync();
            }
        }

        public async Task<ICollection<AdvertViewModel>> GetAllAdvertsAsync()
        {
            return automapper.Map<ICollection<AdvertViewModel>>(await this.dbContext.Adverts
                .ToListAsync());
        }

        public async Task<ICollection<AdvertViewModel>> GetRandomAdvertsAndSanctionThemAsync(int count)
        {
            var advertsQuery = this.dbContext.Adverts.Where(x => x.ViewsLeft > 0);
            var advertsCount = await advertsQuery.CountAsync();

            ICollection<AdvertViewModel> advertsResult = new List<AdvertViewModel>();

            if (advertsCount <= count)
            {
                advertsResult = this.automapper.Map<ICollection<AdvertViewModel>>(await advertsQuery.ToListAsync());
            }
            else
            {
                Random rand = new Random();
                for (int i = 0; i < count; i++)
                {
                    int advertChosen = rand.Next(0, advertsCount - 1);
                    var currentAdvert = advertsQuery.ElementAt(advertChosen);
                    if (currentAdvert != null &&
                       !advertsResult.Any(x => x.Id == currentAdvert.Id))
                    {
                        advertsResult.Add(this.automapper.Map<AdvertViewModel>(currentAdvert));
                        currentAdvert.ViewsLeft -= 1;
                    }
                }
            }
            await this.dbContext.SaveChangesAsync();
            return advertsResult;
        }
    }
}
