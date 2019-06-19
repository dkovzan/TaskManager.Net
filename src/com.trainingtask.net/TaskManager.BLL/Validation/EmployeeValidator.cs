using System.Collections.Generic;
using System.Linq;
using TaskManager.BLL.Exceptions;
using TaskManager.BLL.Models;

namespace TaskManager.BLL.Validation
{
    public class EmployeeValidator
    {
        public void ValidateEmployee (EmployeeDto employee)
        {
            Dictionary<string, string> invalidFieldsWithMessages = new Dictionary<string, string>();

            if (employee.FirstName == null || employee.FirstName == string.Empty)
            {
                invalidFieldsWithMessages.Add("FirstName", "First Name is required");
            }
            else if (employee.FirstName.Length > 255)
            {
                invalidFieldsWithMessages.Add("FirstName", "First Name should containt maximum 255 characters.");
            }

            if (employee.LastName == null || employee.LastName == string.Empty)
            {
                invalidFieldsWithMessages.Add("LastName", "Last Name is required");
            }
            else if (employee.LastName.Length > 255)
            {
                invalidFieldsWithMessages.Add("LastName", "Last Name should contain maximum 255 characters.");
            }

            if (employee.MiddleName.Length > 255)
            {
                invalidFieldsWithMessages.Add("MiddleName", "Middle Name should contain maximum 255 characters.");
            }

            if (employee.Position == null || employee.Position == string.Empty)
            {
                invalidFieldsWithMessages.Add("LastName", "Position is required");
            }
            else if (employee.Position.Length > 255)
            {
                invalidFieldsWithMessages.Add("Position", "Position should contain maximum 255 characters.");
            }

            if (invalidFieldsWithMessages.Any())
            {
                throw new ValidationException("", invalidFieldsWithMessages);
            }
        }
    }
}
