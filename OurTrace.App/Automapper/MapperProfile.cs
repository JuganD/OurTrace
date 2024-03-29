﻿using AutoMapper;
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
using OurTrace.App.Models.ViewModels.Settings;
using OurTrace.Services.Models;
using OurTrace.App.Models.ViewModels.Notification;
using OurTrace.App.Models.ViewModels.Message;
using OurTrace.App.Models.ViewModels.Search;
using OurTrace.App.Automapper.Resolvers;
using OurTrace.App.Models.ViewModels.Advert;
using OurTrace.App.Models.ViewModels.Home;

namespace OurTrace.App.Automapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            UserRelated_Mappings();
            PostRelated_Mappings();
            GroupRelated_Mappings();
            Notification_Mappings();
            Message_Mappings();
            Advert_Mappings();
        }
        private void UserRelated_Mappings()
        {
            CreateMap<OurTraceUser, ProfileViewModel>()
                .ForMember(dest => dest.Years, actual => actual.MapFrom(x => AgeCalculator.GetYears(x.BirthDate ?? DateTime.Now)))
                .ForMember(dest => dest.Following, actual => actual.MapFrom(x => x.Following.Count))
                .ForMember(dest => dest.Followers, actual => actual.MapFrom(x => x.Followers.Count))
                .ForMember(dest => dest.FriendsCount, actual => actual.MapFrom(x =>
                                               x.SentFriendships.Count(y => y.AcceptedOn != null) +
                                               x.ReceivedFriendships.Count(y => y.AcceptedOn != null)))
                .ForMember(dest => dest.Sex, actual => actual.MapFrom(x => x.Sex));

            CreateMap<OurTraceUser, NewsfeedViewModel>()
                .ForMember(dest => dest.FullName, actual => actual.MapFrom(x => x.FullName))
                .ForMember(dest => dest.Following, actual => actual.MapFrom(x => x.Following.Count))
                .ForMember(dest => dest.Followers, actual => actual.MapFrom(x => x.Followers.Count));

            CreateMap<OurTraceUser, SearchResultViewModel>()
                .ForMember(dest => dest.Content, actual => actual.MapFrom(x => x.UserName))
                .ForMember(dest => dest.DescriptiveContent, actual => actual.MapFrom(x => x.FullName));

            CreateMap<RegisterInputModel, OurTraceUser>();
            CreateMap<OurTraceUser, ProfileFriendSuggestionViewModel>();
            CreateMap<OurTraceUser, SettingsFriendRequestViewModel>();
        }
        private void PostRelated_Mappings()
        {
            CreateMap<CreatePostInputModel, Post>()
                .ForMember(x => x.Location, option => option.Ignore());

            CreateMap<Post, PostViewModel>()
                .ForMember(dest => dest.Creator, actual => actual.MapFrom(x => x.User.UserName))
                .ForMember(dest => dest.CreatedOn, actual => actual.MapFrom(x => JsonConvert.SerializeObject(x.CreatedOn)))
                .ForMember(dest => dest.SharedPost, actual => actual.MapFrom(x => x.SharedPost));
            CreateMap<PostLike, PostLikeViewModel>()
                .ForMember(dest => dest.Username, actual => actual.MapFrom(x => x.User.UserName));

            CreateMap<Post, SearchResultViewModel>()
                .ForMember(dest => dest.Content, actual => actual.MapFrom(x => x.User.UserName))
                .ForMember(dest => dest.ContextVariables, actual => actual.MapFrom<DictionaryAddResolver>())
                .ForMember(dest => dest.DescriptiveContent, actual => actual.MapFrom(x => x.Content));

            CreateMap<Share, PostShareViewModel>()
                .ForMember(dest => dest.Username, actual => actual.MapFrom(x => x.User.UserName));

            CreateMap<Comment, CommentViewModel>()
                .ForMember(dest => dest.CreatedOn, actual => actual.MapFrom(x => JsonConvert.SerializeObject(x.CreatedOn)))
                .ForMember(dest => dest.Creator, actual => actual.MapFrom(x => x.User.UserName));

            CreateMap<CommentLike, CommentLikeViewModel>()
                .ForMember(dest => dest.Username, actual => actual.MapFrom(x => x.User.UserName));

            CreateMap<Comment, SearchResultViewModel>()
                .ForMember(dest => dest.Content, actual => actual.MapFrom(x => x.User.UserName))
                .ForMember(dest => dest.ContextVariables, actual => actual.MapFrom<DictionaryAddResolver>())
                .ForMember(dest => dest.DescriptiveContent, actual => actual.MapFrom(x => x.Content));

        }
        private void GroupRelated_Mappings()
        {
            CreateMap<Group, GroupWindowViewModel>()
                .ForMember(dest => dest.Name, act => act.MapFrom(x => x.Name))
                .ForMember(dest => dest.Members, act => act.MapFrom(x => x.Members.Count(y => y.ConfirmedMember == true)))
                .ForMember(dest => dest.Url, act => act.MapFrom(x => x.Url));

            CreateMap<Group, GroupOpenViewModel>()
                .ForMember(dest => dest.CreatedOn, actual => actual.MapFrom(x => JsonConvert.SerializeObject(x.CreatedOn)))
                .ForMember(dest => dest.Members, act => act.MapFrom(x => x.Members.Where(y => y.ConfirmedMember == true)));

            CreateMap<UserGroup, GroupMemberViewModel>()
                .ForMember(dest => dest.Username, act => act.MapFrom(x => x.User.UserName))
                .ForMember(dest => dest.FullName, act => act.MapFrom(x => x.User.FullName))
                .ForMember(dest => dest.JoinedOn, actual => actual.MapFrom(x => JsonConvert.SerializeObject(x.JoinedOn)));

            CreateMap<Group, SearchResultViewModel>()
                .ForMember(dest => dest.Content, actual => actual.MapFrom(x => x.Name))
                .ForMember(dest => dest.DescriptiveContent, actual => actual.MapFrom(x => x.Members.Count + " members"));
        }
        private void Notification_Mappings()
        {
            CreateMap<NotificationServiceModel, Notification>();
            CreateMap<Notification, NotificationViewModel>()
                .ForMember(dest => dest.DateIssued, actual => actual.MapFrom(x => JsonConvert.SerializeObject(x.DateIssued)));
        }
        private void Message_Mappings()
        {
            CreateMap<Message, MessageViewModel>()
                .ForMember(dest => dest.Sender, actual => actual.MapFrom(x => x.Sender.UserName))
                .ForMember(dest => dest.CreatedOn, actual => actual.MapFrom(x => JsonConvert.SerializeObject(x.CreatedOn)));
        }
        private void Advert_Mappings()
        {
            CreateMap<Advert, AdvertViewModel>();
        }
    }
}
