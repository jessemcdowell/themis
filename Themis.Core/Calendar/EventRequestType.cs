using System;

namespace Themis.Calendar
{
    /// <summary>
    /// Types of event requests
    /// </summary>
    public enum EventRequestType
    {
        /// <summary>Invited to an event, or an update to an event invitation</summary>
        Invite,

        /// <summary>Cancels an event</summary>
        Cancel,
    }
}
