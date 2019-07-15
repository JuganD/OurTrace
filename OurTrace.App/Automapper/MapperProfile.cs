﻿using AutoMapper;
using Newtonsoft.Json;
using OurTrace.App.Models.ViewModels;
using OurTrace.App.Models.ViewModels.Comments;
using OurTrace.App.Models.ViewModels.Identity.Profile;
using OurTrace.App.Models.ViewModels.Posts;
using OurTrace.Data.Identity.Models;
using OurTrace.Data.Models;
using OurTrace.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OurTrace.App.Automapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<OurTraceUser, ProfileViewModel>()
                .ForMember(dest => dest.Years, actual => actual.MapFrom(x => AgeCalculator.GetYears(x.BirthDate ?? DateTime.Now)))
                .ForMember(dest => dest.Following, actual => actual.MapFrom(x => x.Following.Count))
                .ForMember(dest => dest.Followers, actual => actual.MapFrom(x => x.Followers.Count));

            CreateMap<Post, PostViewModel>()
                .ForMember(dest => dest.Creator, actual => actual.MapFrom(x => x.User.UserName))
                .ForMember(dest => dest.CreatedOn, actual => actual.MapFrom(x => JsonConvert.SerializeObject(x.CreatedOn)))
                .ForMember(dest => dest.EditedOn, actual => actual.MapFrom(x => JsonConvert.SerializeObject(x.EditedOn)));

            CreateMap<Comment, CommentViewModel>()
                .ForMember(dest => dest.CreatedOn, actual => actual.MapFrom(x => JsonConvert.SerializeObject(x.CreatedOn)))
                .ForMember(dest => dest.EditedOn, actual => actual.MapFrom(x => JsonConvert.SerializeObject(x.EditedOn)))
                .ForMember(dest => dest.Creator, actual => actual.MapFrom(x => x.User.UserName));


        }
    }
}
