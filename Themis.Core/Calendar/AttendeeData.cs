using System;
using System.ComponentModel.DataAnnotations;

namespace Themis.Calendar
{
    /// <summary>
    /// Represents a person or resource that can be invited or otherwise involved with an event.
    /// </summary>
    public class AttendeeData
    {
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }
    }
}
