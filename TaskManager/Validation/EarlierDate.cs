using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace TaskManager.Validation
{
    public class EarlierDateAttribute : ValidationAttribute
    {
        private string laterDateProvided { get; set; }
        public EarlierDateAttribute(string _laterDate)
        {
            laterDateProvided = _laterDate;
        }
        protected override ValidationResult IsValid(object earlierDateValue, ValidationContext validationContext)
        {
            object instance = validationContext.ObjectInstance;

            Type type = instance.GetType();

            DateTime earlierDate = Convert.ToDateTime(earlierDateValue);

            PropertyInfo laterDatePropInfo = type.GetProperty(laterDateProvided);

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
}