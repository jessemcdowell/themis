using System;

namespace Themis.Calendar
{
    public class VCalendarNotFoundException : Exception
    {
        public VCalendarNotFoundException(string message)
            : base(message)
        { }

        public VCalendarNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
