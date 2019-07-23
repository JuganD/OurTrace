using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace OurTrace.App.Models.Attributes.Validation
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class FileExtensionAttribute : ValidationAttribute
    {
        private List<string> AllowedExtensions { get; set; }

        public FileExtensionAttribute(string fileExtensions)
        {
            AllowedExtensions = fileExtensions.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public override bool IsValid(object value)
        {
            if (value is IFormFile)
            {
                IFormFile file = value as IFormFile;
                if (file != null)
                {
                    var fileName = file.FileName;

                    return AllowedExtensions.Any(y => fileName.EndsWith(y));
                }
            }
            else if (value is string)
            {
                var file = value as string;
                if (file != null)
                {
                    return AllowedExtensions.Any(y => file.EndsWith(y));
                }
            }

            return true;
        }
    }
}
