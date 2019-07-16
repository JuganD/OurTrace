using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace OurTrace.App.Models.Attributes.Validation
{
    public class EnumRangeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value?.GetType() != typeof(Enum)) throw new InvalidOperationException("Not the proper value.");

            string[] AllowableValues = Enum.GetNames(value.GetType());

            string strValue = Enum.GetName(value.GetType(), value);

            if (AllowableValues?.Contains(strValue) == true)
            {
                return ValidationResult.Success;
            }

            var msg = $"Please enter one of the allowable values: {string.Join(", ", (AllowableValues ?? new string[] { "No allowable values found" }))}.";
            return new ValidationResult(msg);
        }
    }
}
