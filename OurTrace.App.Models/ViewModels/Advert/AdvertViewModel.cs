using OurTrace.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OurTrace.App.Models.ViewModels.Advert
{
    public class AdvertViewModel
    {
        public string Id { get; set; }
        public AdvertType Type { get; set; }
        public string IssuerName { get; set; }
        public string Content { get; set; }
        public int ViewsLeft { get; set; }
    }
}
