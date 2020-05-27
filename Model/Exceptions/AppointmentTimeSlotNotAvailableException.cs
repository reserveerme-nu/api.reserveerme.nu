using System;

namespace Model.Exceptions
{
    public class AppointmentTimeSlotNotAvailableException : Exception
    {
        public AppointmentTimeSlotNotAvailableException()
        {
        }

        public AppointmentTimeSlotNotAvailableException(string message)
            : base(message)
        {
        }

        public AppointmentTimeSlotNotAvailableException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}