using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TaskManager.Validation
{
    public class MinDateAttribute : ValidationAttribute
    {
        private readonly string minDate;

        public MinDateAttribute(string _minDate)
        {
            minDate = _minDate;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime minDateValue = Convert.ToDateTime(value);

            DateTime minDateFromAnnotation = Convert.ToDateTime(minDate);

            //try
            //{
            //    DateTime.TryParse(minDate, out minDateFromAnnotation);
            //}
            //catch (FormatException)
            //{
            //    return new ValidationResult(ErrorMessage);
            //}

            if (minDateValue < minDateFromAnnotation)
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