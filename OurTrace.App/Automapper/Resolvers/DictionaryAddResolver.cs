using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using OurTrace.App.Models.ViewModels.Search;
using OurTrace.Data.Models;

namespace OurTrace.App.Automapper.Resolvers
{
    public class DictionaryAddResolver : IValueResolver<Comment, SearchResultViewModel, IDictionary<string,string>>, IValueResolver<Post, SearchResultViewModel, IDictionary<string, string>>
    {

        public IDictionary<string, string> Resolve(Comment source, SearchResultViewModel destination, IDictionary<string, string> destMember, ResolutionContext context)
        {
            destMember.Add("Id", source.Post.Id);
            return destMember;
        }

        public IDictionary<string, string> Resolve(Post source, SearchResultViewModel destination, IDictionary<string, string> destMember, ResolutionContext context)
        {
            destMember.Add("Id", source.Id);
            return destMember;
        }
    }
}
