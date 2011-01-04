using System;

namespace Themis.Calendar
{
    /// <summary>
    /// Indicates that the content violates the VCalendar formatting rules
    /// </summary>
    public class VCalendarFormatException : Exception
    {
        public VCalendarFormatException(string message)
            : base(message)
        { }

        public VCalendarFormatException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
