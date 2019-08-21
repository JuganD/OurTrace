using System.Collections.Generic;

namespace OurTrace.App.Models.ViewModels.Profile
{
    public class ProfileLastPicturesViewModel
    {
        public ProfileLastPicturesViewModel()
        {
            this.ExternalUrls = new List<string>();
            this.InternalIds = new List<string>();
        }
        public ICollection<string> ExternalUrls { get; set; }
        public ICollection<string> InternalIds { get; set; }
    }
}
