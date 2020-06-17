using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Exceptions
{
    public class RoomNotFoundException : Exception
    {
        public RoomNotFoundException()
        {
        }

        public RoomNotFoundException(string message)
            : base(message)
        {
        }

        public RoomNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
