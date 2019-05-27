using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using TaskManager.Services;

namespace TaskManager.Validation
{
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

            if (_projectService.isProjectShortNameUnique(id, value.ToString()))
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

            Type type = instance.GetType();

            PropertyInfo idPropInfo = type.GetProperty("Id");

            return Convert.ToInt32(idPropInfo.GetValue(instance));
        }

    }
}