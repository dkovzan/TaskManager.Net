using System;

namespace TaskManager.Exceptions
{
    public class DaoException : Exception
    {
        public DaoException(string message) : base(message)
        {
        }
    }
}