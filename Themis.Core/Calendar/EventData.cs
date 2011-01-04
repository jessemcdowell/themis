using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Themis.Calendar
{
    /// <summary>
    /// Describes an event
    /// </summary>
    public class EventData
    {
        [Required]
        public string EventId { get; set; }

        [Required]
        public DateTime Start { get; set; }

        [Required]
        public DateTime End { get; set; }

        [Required]
        public AttendeeData Organizer { get; set; }

        /// <summary>
        /// A brief description ("title") of the event.
        /// </summary>
        public string Summary { get; set; }
    }
}
