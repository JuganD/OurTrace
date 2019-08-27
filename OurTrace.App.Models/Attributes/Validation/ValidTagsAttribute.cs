using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace OurTrace.App.Models.Attributes.Validation
{
    public class ValidTagsAttribute : ValidationAttribute
    {
        public ValidTagsAttribute(string ErrorMessage)
        {
            this.ErrorMessage = ErrorMessage;
        }
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            if (value == null) return ValidationResult.Success;
            if (value.GetType() != typeof(string)) throw new InvalidOperationException("Invalid data.");

            if (((string)value).Split(',').All(x => x.Length >= 3))
                return ValidationResult.Success;
            else
                return new ValidationResult(this.ErrorMessage);
        }
    }
}
