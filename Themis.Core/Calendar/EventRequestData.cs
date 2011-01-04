using System;
using System.ComponentModel.DataAnnotations;

namespace Themis.Calendar
{
    /// <summary>
    /// Describes a request regarding an event
    /// </summary>
    public class EventRequestData
    {
        [Required]
        public EventRequestType RequestType { get; set; }

        [Required]
        public EventData Event { get; set; }
    }
}
