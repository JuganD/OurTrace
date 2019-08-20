using AutoMapper;
using Newtonsoft.Json;
using OurTrace.App.Models.ViewModels.Comments;
using OurTrace.App.Models.ViewModels.Profile;
using OurTrace.App.Models.ViewModels.Post;
using OurTrace.Data.Identity.Models;
using OurTrace.Data.Models;
using OurTrace.Services.Helpers;
using System;
using System.Linq;
using OurTrace.App.Models.Authenticate;
using OurTrace.App.Models.InputModels.Posts;
using OurTrace.App.Models.ViewModels.Group;

namespace OurTrace.App.Automapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<OurTraceUser, ProfileViewModel>()
                .ForMember(dest => dest.Years, actual => actual.MapFrom(x => AgeCalculator.GetYears(x.BirthDate ?? DateTime.Now)))
                .ForMember(dest => dest.Following, actual => actual.MapFrom(x => x.Following.Count))
                .ForMember(dest => dest.Followers, actual => actual.MapFrom(x => x.Followers.Count))
                .ForMember(dest => dest.FriendsCount, actual => actual.MapFrom(x =>
                                               x.SentFriendships.Count(y => y.AcceptedOn != null) +
                                               x.ReceivedFriendships.Count(y => y.AcceptedOn != null)))
                .ForMember(dest => dest.Sex, actual => actual.MapFrom(x => x.Sex));

            CreateMap<RegisterInputModel, OurTraceUser>();
            CreateMap<CreatePostInputModel, Post>()
                .ForMember(x => x.Location, option => option.Ignore());

            CreateMap<Post, PostViewModel>()
                .ForMember(dest => dest.Creator, actual => actual.MapFrom(x => x.User.UserName))
                .ForMember(dest => dest.CreatedOn, actual => actual.MapFrom(x => JsonConvert.SerializeObject(x.CreatedOn)))
                .ForMember(dest => dest.EditedOn, actual => actual.MapFrom(x => JsonConvert.SerializeObject(x.EditedOn)))
                .ForMember(dest => dest.Likes, actual => actual.MapFrom(x => x.Likes.Count))
                .ForMember(dest => dest.Shares, actual => actual.MapFrom(x => x.Shares.Count));

            CreateMap<Comment, CommentViewModel>()
                .ForMember(dest => dest.CreatedOn, actual => actual.MapFrom(x => JsonConvert.SerializeObject(x.CreatedOn)))
                .ForMember(dest => dest.EditedOn, actual => actual.MapFrom(x => JsonConvert.SerializeObject(x.EditedOn)))
                .ForMember(dest => dest.Creator, actual => actual.MapFrom(x => x.User.UserName));

            CreateMap<Group, GroupWindowViewModel>()
                .ForMember(dest => dest.Name, act => act.MapFrom(x => x.Name))
                .ForMember(dest => dest.Members, act => act.MapFrom(x => x.Members.Count))
                .ForMember(dest => dest.Url, act => act.MapFrom(x => x.Url));

            CreateMap<Group, GroupOpenViewModel>()
                .ForMember(dest => dest.CreatedOn, actual => actual.MapFrom(x => JsonConvert.SerializeObject(x.CreatedOn)))
                .ForMember(dest => dest.Members, act => act.MapFrom(x => x.Members.Where(y=>y.ConfirmedMember==true)));

            CreateMap<UserGroup, GroupMemberViewModel>()
                .ForMember(dest => dest.Username, act => act.MapFrom(x => x.User.UserName))
                .ForMember(dest => dest.FullName, act => act.MapFrom(x => x.User.FullName))
                .ForMember(dest => dest.JoinedOn, actual => actual.MapFrom(x => JsonConvert.SerializeObject(x.JoinedOn)));
        }
    }
}
