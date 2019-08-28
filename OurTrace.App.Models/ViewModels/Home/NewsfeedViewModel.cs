using AutoMapper.Configuration.Annotations;
using OurTrace.App.Models.ViewModels.Advert;
using OurTrace.App.Models.ViewModels.Post;
using OurTrace.App.Models.ViewModels.Profile;
using System.Collections.Generic;

namespace OurTrace.App.Models.ViewModels.Home
{
    public class NewsfeedViewModel
    {
        public NewsfeedViewModel()
        {
            this.Posts = new List<PostViewModel>();
        }
        public string FullName { get; set; }
        public int Followers { get; set; }
        public int Following { get; set; }
        [Ignore]
        public ICollection<PostViewModel> Posts { get; set; }
    }
}
