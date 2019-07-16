using System;
using System.ComponentModel.DataAnnotations;

namespace OurTrace.App.Models.Attributes.Validation
{
    public class IsTrueAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null) return false;
            if (value.GetType() != typeof(bool)) throw new InvalidOperationException("Invalid data.");

            return (bool)value;
        }
    }
}
