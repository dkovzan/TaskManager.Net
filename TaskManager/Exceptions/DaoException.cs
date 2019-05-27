using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskManager.Exceptions
{
    public class DaoException : Exception
    {
        public DaoException(string message) : base(message)
        {
        }
    }
}