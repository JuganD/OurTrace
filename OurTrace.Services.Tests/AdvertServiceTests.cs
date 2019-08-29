namespace OurTrace.Services.Tests
{
    using Microsoft.EntityFrameworkCore;
    using OurTrace.Data;
    using OurTrace.Data.Models;
    using System;
    using Xunit;
    using AutoMapper;
    using System.Threading.Tasks;
    using OurTrace.App.Models.Advert;
    using OurTrace.Services.Tests.StaticResources;
    using System.Linq;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class AdvertServiceTests
    {
        private readonly IMapper automapper;
        private readonly AdvertService advertService;
        private readonly OurTraceDbContext dbContext;

        public AdvertServiceTests()
        {
            var options = new DbContextOptionsBuilder<OurTraceDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString())
               .Options;
            this.dbContext = new OurTraceDbContext(options);

            this.automapper = MapperInitializer.CreateMapper();

            this.advertService = new AdvertService(dbContext, automapper);
        }
        [Fact]
        public async Task AddAsync_ShouldAddAddon_Succesfully()
        {
            var advert1 = new ModifyAdvertInputModel()
            {
                Content = "Advcontent",
                IssuerName = "Gosho",
                Type = AdvertType.Group,
                ViewsLeft = 50
            };
            var advert2 = new ModifyAdvertInputModel()
            {
                Content = "ADV CONTENT 2",
                IssuerName = "Pesho",
                Type = AdvertType.User,
                ViewsLeft = 1
            };

            await this.advertService.AddAdvertAsync(advert1);
            await this.advertService.AddAdvertAsync(advert2);

            var advertsCount = await this.dbContext.Adverts.CountAsync();
            Assert.Equal(2, advertsCount);

            var advert1Result = await this.dbContext.Adverts.SingleOrDefaultAsync(x => x.IssuerName == advert1.IssuerName);
            var advert2Result = await this.dbContext.Adverts.SingleOrDefaultAsync(x => x.IssuerName == advert2.IssuerName);

            Assert.Equal(advert1.Content, advert1Result.Content);
            Assert.Equal(advert1.IssuerName, advert1Result.IssuerName);
            Assert.Equal(advert1.ViewsLeft, advert1Result.ViewsLeft);
            Assert.Equal(advert1.Type, advert1Result.Type);

            Assert.Equal(advert2.Content, advert2Result.Content);
            Assert.Equal(advert2.IssuerName, advert2Result.IssuerName);
            Assert.Equal(advert2.ViewsLeft, advert2Result.ViewsLeft);
            Assert.Equal(advert2.Type, advert2Result.Type);
        }
        [Fact]
        public async Task AdvertExistsAsync_ShouldReturn_True()
        {
            var advert = new Advert()
            {
                Id = "1234",
                Content = "Advcontent",
                IssuerName = "Gosho",
                Type = AdvertType.Group,
                ViewsLeft = 50
            };

            await this.dbContext.Adverts.AddAsync(advert);
            await this.dbContext.SaveChangesAsync();

            var advertsCount = await this.dbContext.Adverts.CountAsync();
            Assert.Equal(1, advertsCount);

            var actual = await this.advertService.AdvertExistsAsync("1234");
            Assert.True(actual);
        }
        [Fact]
        public async Task GetAllAdvertsAsync_ShouldReturn_Succesfully()
        {
            var advert = new Advert()
            {
                Id = "1234",
                Content = "Advcontent",
                IssuerName = "Gosho",
                Type = AdvertType.Group,
                ViewsLeft = 50
            };
            var advert2 = new Advert()
            {
                Id = "12345",
                Content = "Advcontent2",
                IssuerName = "Pesho",
                Type = AdvertType.User,
                ViewsLeft = 0
            };
            var advert3 = new Advert()
            {
                Id = "123456",
                Content = "Advcontent3",
                IssuerName = "Mitko",
                Type = AdvertType.User,
                ViewsLeft = -5
            };

            await this.dbContext.Adverts.AddAsync(advert);
            await this.dbContext.Adverts.AddAsync(advert2);
            await this.dbContext.Adverts.AddAsync(advert3);
            await this.dbContext.SaveChangesAsync();

            var actual = await this.advertService.GetAllAdvertsAsync();
            Assert.Equal(3, actual.Count);
        }
        [Fact]
        public async Task GetRandomAdvertsAndSanctionThemAsync_ShouldReturn_AtLeastOne_And_SanctionIt()
        {
            var advertCollection = new List<Advert>() {
            new Advert()
            {
                Id = "1234",
                Content = "Advcontent",
                IssuerName = "Gosho",
                Type = AdvertType.Group,
                ViewsLeft = 50
            },
            new Advert()
            {
                Id = "12345",
                Content = "Advcontent2",
                IssuerName = "Pesho",
                Type = AdvertType.User,
                ViewsLeft = 0
            },
            new Advert()
            {
                Id = "123456",
                Content = "Advcontent3",
                IssuerName = "Mitko",
                Type = AdvertType.User,
                ViewsLeft = 2
            },
            new Advert()
            {
                Id = "1234567",
                Content = "Advcontent34",
                IssuerName = "Mitko54",
                Type = AdvertType.Group,
                ViewsLeft = -5
            }
            };

            await this.dbContext.Adverts.AddRangeAsync(advertCollection);
            await this.dbContext.SaveChangesAsync();

            // Should return advert 1 or advert 3 or both (its Random), 
            // because they have positive ViewsLeft
            var expected1Id = "123456";
            var expected2Id = "1234";


            var actual = await this.advertService.GetRandomAdvertsAndSanctionThemAsync(2);
            Assert.True(actual.Count > 0);
            Assert.Contains(actual, x => x.Id == expected1Id || x.Id == expected2Id);

            var expectedValue1 = 1;
            var expectedValue2 = 49;

            var actualDatabaseValues = await this.dbContext.Adverts.Where(x => x.Id == expected1Id || x.Id == expected2Id).ToListAsync();
            Assert.Contains(actualDatabaseValues, x => x.ViewsLeft == expectedValue1 || x.ViewsLeft == expectedValue2);
        }
        [Fact]
        public async Task ModifyAdvertByIdAsync_ShouldModify_Succesfully()
        {
            var advert = new Advert()
            {
                Id = "1234",
                Content = "Advcontent",
                IssuerName = "Gosho",
                Type = AdvertType.Group,
                ViewsLeft = 50
            };
            var advertInputModel = new ModifyAdvertInputModel()
            {
                Id = "1234",
                Content = "ModifiedContent",
                IssuerName = "Mitko",
                Type = AdvertType.User,
                ViewsLeft = 0
            };

            await this.dbContext.Adverts.AddAsync(advert);
            await this.dbContext.SaveChangesAsync();


            var result = await this.advertService.ModifyAdvertByIdAsync(advertInputModel);
            Assert.True(result);

            var expected = new Advert()
            {
                Id = "1234",
                Content = "ModifiedContent",
                IssuerName = "Mitko",
                Type = AdvertType.User,
                ViewsLeft = 0
            };
            var actual = await this.dbContext.Adverts.SingleOrDefaultAsync(x => x.Id == expected.Id);

            // Easier to compare all properties
            var expectedString = JsonConvert.SerializeObject(expected);
            var actualString = JsonConvert.SerializeObject(actual);
            Assert.Equal(expectedString, actualString);
        }
    }
}
