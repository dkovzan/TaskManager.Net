using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace TaskManager.Services
{
    public class ValidationService
    {
    }

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

            PropertyInfo laterDatePropInfo = instance.GetType().GetProperty(laterDateProvided);

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

    public class UniqueForProjectsAttribute : ValidationAttribute
    {
        private readonly ProjectService _projectService;
        public UniqueForProjectsAttribute()
        {
            _projectService = new ProjectService();
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            int id = GetIdFromValidationContext(validationContext);

            if (_projectService.IsProjectShortNameUnique(id, (string) value ?? string.Empty))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(ErrorMessage);
            }
        }

        private int GetIdFromValidationContext(ValidationContext validationContext)
        {
            object instance = validationContext.ObjectInstance;

            PropertyInfo idPropInfo = instance.GetType().GetProperty("Id");

            return Convert.ToInt32(idPropInfo.GetValue(instance));
        }

    }
}