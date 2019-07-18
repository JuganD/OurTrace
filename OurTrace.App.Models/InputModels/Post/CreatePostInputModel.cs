using AutoMapper.Configuration.Annotations;
using Microsoft.AspNetCore.Http;
using OurTrace.App.Models.Attributes.Validation;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OurTrace.App.Models.InputModels.Posts
{
    public class CreatePostInputModel
    {
        [Required]
        [StringLength(500, MinimumLength = 5, ErrorMessage ="Post content minimum length is 5 characters")]
        public string Content { get; set; }

        [FileExtension("jpg,jpeg,png,bmp", ErrorMessage = "Wrong image extension.")]
        public IFormFile MediaFile { get; set; }

        public string Location { get; set; }

        [RegularExpression("(?:^)((?:(?:[\\w\\.]+)(?:, ?|$))+)", ErrorMessage ="Tags must be words (without special symbols), separated by comma ','")]
        public string Tags { get; set; }
    }
}
