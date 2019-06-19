using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace TaskManager.WEB.Validation
{
    public class EarlierDate : ValidationAttribute
    {
        private readonly string laterDatePropName;
        public EarlierDate(string _laterDate)
        {
            laterDatePropName = _laterDate;
        }
        protected override ValidationResult IsValid(object earlierDateValue, ValidationContext validationContext)
        {
            object instance = validationContext.ObjectInstance;

            PropertyInfo laterDatePropInfo = instance.GetType().GetProperty(laterDatePropName);

            DateTime earlierDate = Convert.ToDateTime(earlierDateValue);

            DateTime laterDate = Convert.ToDateTime(laterDatePropInfo.GetValue(instance));

            if (earlierDate.Date <= laterDate.Date)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(ErrorMessage);
            }
        }
    }

    public class MaxDate : ValidationAttribute
    {
        private readonly string maxDate;

        public MaxDate(string _maxDate)
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

    public class MinDate : ValidationAttribute
    {
        private readonly string minDate;

        public MinDate(string _minDate)
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