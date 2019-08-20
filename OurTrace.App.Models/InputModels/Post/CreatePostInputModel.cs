using AutoMapper.Configuration.Annotations;
using Microsoft.AspNetCore.Http;
using OurTrace.App.Models.Attributes.Validation;
using OurTrace.Data.Models;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OurTrace.App.Models.InputModels.Posts
{
    public class CreatePostInputModel
    {
        public CreatePostInputModel()
        {
            this.Errors = new List<string>();
        }
        [Required(ErrorMessage ="The post must have some content.")]
        [StringLength(1000, MinimumLength = 5, ErrorMessage ="Post content minimum length is 5 characters and maximum length it 1000 characters")]
        public string Content { get; set; }

        [Ignore]
        [FileExtension("jpg,jpeg,png,bmp", ErrorMessage = "Wrong image extension.")]
        public IFormFile MediaFile { get; set; }

        [Ignore]
        [FileExtension("jpg,jpeg,png,bmp", ErrorMessage = "Wrong external image extension.")]
        public string ExternalMediaUrl { get; set; }

        [Ignore]
        public string Location { get; set; }

        [Ignore]
        [RegularExpression("(?:^)((?:(?:[\\w\\.]+)(?:, ?|$))+)", ErrorMessage ="Tags must be words (without special symbols), separated by comma ','")]
        public string Tags { get; set; }

        public PostVisibilityType VisibilityType { get; set; }

        [Ignore]
        public ICollection<string> Errors { get; set; }

        [Ignore]
        public string PostState { get; set; }

        [Ignore]
        public bool IsGroupPost { get; set; }
    }
}
