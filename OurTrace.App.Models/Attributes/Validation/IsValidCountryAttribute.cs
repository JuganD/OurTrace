using OurTrace.App.Helpers;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace OurTrace.App.Models.Attributes.Validation
{
    public class IsValidCountryAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null) return false;
            if (value.GetType() != typeof(string)) throw new InvalidOperationException("Not a valid country text.");

            if (CountryHelper.Countries.Contains((string)value))
            {
                return true;
            }

            return false;
        }
    }
}
