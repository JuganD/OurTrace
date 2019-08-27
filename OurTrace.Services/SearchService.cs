using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OurTrace.App.Models.ViewModels.Search;
using OurTrace.Data;
using OurTrace.Services.Abstraction;
using OurTrace.Services.Helpers;
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
        private readonly IPostService postService;

        public SearchService(OurTraceDbContext dbContext,
            IMapper automapper,
            IPostService postService)
        {
            this.dbContext = dbContext;
            this.automapper = automapper;
            this.postService = postService;
        }
        public async Task<SearchDefaultViewModel> SearchForCommentsAsync(string query, string username)
        {
            query = query.ToLowerInvariant();

            var result = await this.dbContext.Comments
                .Include(x => x.User)
                .Include(x => x.Post)
                .Where(x => x.Content != null &&
                            x.Content.IndexOf(query, StringComparison.CurrentCultureIgnoreCase) >= 0)
                .ToListAsync();

            if (result == null) return null;

            result = result
                .Where(x => this.postService
                        .IsUserCanSeePostAsync(username, x.Post.Id)
                            .GetAwaiter().GetResult())
                .ToList();


            return new SearchDefaultViewModel() { Values = this.automapper.Map<ICollection<SearchResultViewModel>>(result) };
        }

        public async Task<SearchDefaultViewModel> SearchForGroupsAsync(string query)
        {
            query = query.ToLowerInvariant();

            var result = await this.dbContext.Groups
                .Include(x => x.Members)
                .Where(x => x.Name != null &&
                            x.Name.IndexOf(query, StringComparison.CurrentCultureIgnoreCase) >= 0)
                .ToListAsync();

            if (result == null) return null;

            return new SearchDefaultViewModel() { Values = this.automapper.Map<ICollection<SearchResultViewModel>>(result) };
        }

        public async Task<SearchDefaultViewModel> SearchForUsersAsync(string query)
        {
            query = query.ToLowerInvariant();

            var result = await this.dbContext.Users
                .Where(x => (x.UserName != null &&
                            x.UserName.IndexOf(query, StringComparison.CurrentCultureIgnoreCase) >= 0) ||
                          (x.FullName != null &&
                            x.FullName.IndexOf(query, StringComparison.CurrentCultureIgnoreCase) >= 0))
                .ToListAsync();

            if (result == null) return null;

            return new SearchDefaultViewModel() { Values = this.automapper.Map<ICollection<SearchResultViewModel>>(result) };
        }

        public async Task<SearchDefaultViewModel> SearchForPostsAsync(string query, string username)
        {
            // Double result variables because of non-materialized Tags conversion
            // which causes nullref when used with content.contains
            query = query.ToLowerInvariant();

            var resultPosts = await this.dbContext.Posts
                .Include(x => x.User)
                .Where(x => x.Content != null &&
                            x.Content.IndexOf(query, StringComparison.CurrentCultureIgnoreCase) >= 0)
                .ToListAsync();

            var resultTags = await this.dbContext.Posts
                .Include(x => x.User)
                .Where(x => x.Tags != null &&
                            x.Tags.Select(y => y.ToLowerInvariant()).Contains(query))
                .ToListAsync();

            if (resultPosts == null && resultTags == null) return null;

            resultPosts = resultPosts
                .Where(x => this.postService
                        .IsUserCanSeePostAsync(username, x.Id)
                            .GetAwaiter().GetResult())
                .ToList();

            return new SearchDefaultViewModel() { Values = this.automapper.Map<ICollection<SearchResultViewModel>>(resultPosts.Union(resultTags).ToList()) };
        }

        public async Task<SearchAllViewModel> SearchForEverythingAsync(string query, string username)
        {
            var result = new SearchAllViewModel();
            result.Users = (await SearchForUsersAsync(query)).Values;
            result.Groups = (await SearchForGroupsAsync(query)).Values;
            result.Posts = (await SearchForPostsAsync(query, username)).Values;
            result.Comments = (await SearchForCommentsAsync(query, username)).Values;

            return result;
        }
    }
}
