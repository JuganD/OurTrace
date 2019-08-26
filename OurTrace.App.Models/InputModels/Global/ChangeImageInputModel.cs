using Microsoft.AspNetCore.Http;
using OurTrace.App.Models.Attributes.Validation;

namespace OurTrace.App.Models.InputModels.Global
{
    public class ChangeImageInputModel
    {
        public bool IsGroup { get; set; }
        public string GroupName { get; set; }
        [FileExtension("jpg,jpeg,png,bmp", ErrorMessage = "Wrong image extension.")]
        public IFormFile FrontImageMediaFile { get; set; }

        [FileExtension("jpg,jpeg,png,bmp", ErrorMessage = "Wrong image extension.")]
        public IFormFile CoverImageMediaFile { get; set; }
    }
}
