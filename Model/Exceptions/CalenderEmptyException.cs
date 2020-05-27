using System;

namespace Model.Exceptions
{
    public class CalenderEmptyException : Exception
    {
        public CalenderEmptyException()
        {
        }

        public CalenderEmptyException(string message)
            : base(message)
        {
        }

        public CalenderEmptyException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}