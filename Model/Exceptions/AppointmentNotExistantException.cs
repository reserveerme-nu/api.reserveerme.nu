using System;

namespace Model.Exceptions
{
    public class AppointmentNotExistantException : Exception
    {
        public AppointmentNotExistantException()
        {
        }
        
        public AppointmentNotExistantException(string message)
            : base(message)
        {
        }
        
        public AppointmentNotExistantException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}