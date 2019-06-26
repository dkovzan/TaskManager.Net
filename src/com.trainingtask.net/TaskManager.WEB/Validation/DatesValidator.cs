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
        private readonly DateTime maxDate;

        public MaxDate(int year, int month, int day)
        {
            maxDate = new DateTime (year, month, day);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime maxDateValue = Convert.ToDateTime(value);

            DateTime maxDateFromAnnotation = maxDate;

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
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime minDateValue = Convert.ToDateTime(value);

            DateTime currentDate = DateTime.Now;

            if (minDateValue <= currentDate)
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