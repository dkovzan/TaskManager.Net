using System;
using System.ComponentModel.DataAnnotations;

namespace TaskManager.WEB.Validation
{
    public class EarlierDate : ValidationAttribute
    {
        private readonly string _laterDatePropName;
        public EarlierDate(string laterDate)
        {
            _laterDatePropName = laterDate;
        }
        protected override ValidationResult IsValid(object earlierDateValue, ValidationContext validationContext)
        {
            var instance = validationContext.ObjectInstance;

            var laterDatePropInfo = instance.GetType().GetProperty(_laterDatePropName);

            var earlierDate = Convert.ToDateTime(earlierDateValue);

            if (laterDatePropInfo == null)
                throw new ArgumentException("Provided field does not exist in view model.");

            var laterDate = Convert.ToDateTime(laterDatePropInfo.GetValue(instance));

            return earlierDate.Date <= laterDate.Date ? ValidationResult.Success : new ValidationResult(ErrorMessage);

        }
    }

    public class MaxDate : ValidationAttribute
    {
        private readonly DateTime _maxDate;

        public MaxDate(int year, int month, int day)
        {
            _maxDate = new DateTime (year, month, day);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var maxDateValue = Convert.ToDateTime(value);

            return maxDateValue.Date > _maxDate.Date ? new ValidationResult(ErrorMessage) : ValidationResult.Success;

        }
    }

    public class MinDate : ValidationAttribute
    {
        private readonly DateTime _minDate;

        public MinDate(int year, int month, int day)
        {
            _minDate = new DateTime(year, month, day);
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var minDateValue = Convert.ToDateTime(value);

            return minDateValue.Date < _minDate.Date ? new ValidationResult(ErrorMessage) : ValidationResult.Success;

        }
    }

}