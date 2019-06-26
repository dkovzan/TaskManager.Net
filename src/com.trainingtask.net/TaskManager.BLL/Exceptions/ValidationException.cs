using System;
using System.Collections.Generic;

namespace TaskManager.BLL.Exceptions
{
    public class ValidationException : Exception
    {
        public Dictionary<string, string> InvalidFieldsWithMessages { get; set; }

        public ValidationException(string message, Dictionary<string, string> invalidFieldsWithMessages) : base(message)
        {
            InvalidFieldsWithMessages = invalidFieldsWithMessages;
        }
    }
}
