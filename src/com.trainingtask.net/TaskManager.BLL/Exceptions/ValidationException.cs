using System;
using System.Collections.Generic;

namespace TaskManager.BLL.Exceptions
{
    public class ValidationException : Exception
    {
        public Dictionary<string, string> _invalidFieldsWithMessages { get; set; }

        public ValidationException(string message, Dictionary<string, string> invalidFieldsWithMessages) : base(message)
        {
            _invalidFieldsWithMessages = invalidFieldsWithMessages;
        }
    }
}
