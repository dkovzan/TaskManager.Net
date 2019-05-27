using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TaskManager.Validation
{
    public class MaxDateAttribute : ValidationAttribute
    {
        private readonly string maxDate;

        public MaxDateAttribute(string _maxDate)
        {
            maxDate = _maxDate;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime maxDateValue = Convert.ToDateTime(value);

            DateTime maxDateFromAnnotation = Convert.ToDateTime(maxDate);

            //try
            //{
            //    DateTime.TryParse(value, out maxDateFromAnnotation);
            //}
            //catch (FormatException)
            //{
            //    return new ValidationResult(ErrorMessage);
            //}

            if (maxDateValue > maxDateFromAnnotation)
            {
                return new ValidationResult(ErrorMessage);
            }
            else
            {
                return ValidationResult.Success;
            }

        }
    }
}