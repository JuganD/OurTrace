using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OurTrace.App.Models.ViewModels.Search;
using OurTrace.Data;
using OurTrace.Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurTrace.Services
{
    public class SearchService : ISearchService
    {
        private readonly OurTraceDbContext dbContext;
        private readonly IMapper automapper;

        public SearchService(OurTraceDbContext dbContext,
            IMapper automapper)
        {
            this.dbContext = dbContext;
            this.automapper = automapper;
        }
        public async Task<ICollection<SearchResultViewModel>> SearchForCommentsAsync(string query)
        {
            var result = await this.dbContext.Comments
                .Where(x => x.Content.Contains(query))
                .ToListAsync();

            return this.automapper.Map<ICollection<SearchResultViewModel>>(result);
        }

        public async Task<ICollection<SearchResultViewModel>> SearchForGroupsAsync(string query)
        {
            var result = await this.dbContext.Groups
                .Where(x => x.Name.Contains(query))
                .ToListAsync();

            return this.automapper.Map<ICollection<SearchResultViewModel>>(result);
        }

        public async Task<ICollection<SearchResultViewModel>> SearchForPeopleAsync(string query)
        {
            var result = await this.dbContext.Users
                .Where(x => x.UserName.Contains(query) ||
                          x.FullName.Contains(query))
                .ToListAsync();

            return this.automapper.Map<ICollection<SearchResultViewModel>>(result);
        }

        public async Task<ICollection<SearchResultViewModel>> SearchForPostsAsync(string query)
        {
            var result = await this.dbContext.Posts
                .Where(x => x.Content.Contains(query) ||
                          x.Tags.Contains(query))
                .ToListAsync();

            return this.automapper.Map<ICollection<SearchResultViewModel>>(result);
        }

        public async Task<IDictionary<SearchResultType, ICollection<SearchResultViewModel>>> SearchForEverythingAsync(string query)
        {
            var result = new Dictionary<SearchResultType, ICollection<SearchResultViewModel>>();
            result.Add(SearchResultType.User, await SearchForPeopleAsync(query));
            result.Add(SearchResultType.Group, await SearchForGroupsAsync(query));
            result.Add(SearchResultType.Post, await SearchForPostsAsync(query));
            result.Add(SearchResultType.Comment, await SearchForCommentsAsync(query));

            return result;
        }
    }
}
