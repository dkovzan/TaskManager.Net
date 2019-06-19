using System;

namespace TaskManager.BLL.Exceptions
{
    public class DaoException : Exception
    {
        public DaoException(string message) : base(message)
        {
        }
    }
}