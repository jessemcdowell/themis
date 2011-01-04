using System;
using Themis.Email;
using Themis.Calendar.VCard;

namespace Themis.Calendar
{
    /// <summary>
    /// Reads Calendar Request data in the VCalendar format
    /// </summary>
    public class VCalendarRequestParser
    {
        public EventRequestData GetEventRequestFromEmail(IReceivedEmail email)
        {
            throw new NotImplementedException();
        }

        public EventRequestData GetEventRequestFromVCard(VCardGroup document)
        {
            EventRequestData er = new EventRequestData();

            // check the version
            VCardValue version;
            if (!TryGetValue(document, "VERSION", out version))
                throw new VCalendarFormatException("VERSION value not found.");
            if (!String.Equals(version.GetText(), "2.0", StringComparison.InvariantCulture))
                throw new VCalendarFormatException("VCalendar version not supported");

            // request type
            VCardValue method;
            if (!TryGetValue(document, "METHOD", out method))
                throw new VCalendarFormatException("METHOD value not found");
            string methodText = method.GetText();

            er.RequestType = GetRequestTypeFromMethod(methodText);


            // event
            VCardEntity eventEntity;
            if (!document.Children.TryGetFirstEntity("VEVENT", out eventEntity) ||
                !(eventEntity is VCardGroup))
                throw new VCalendarFormatException("VEVENT group not found");

            er.Event = GetEvent((VCardGroup)eventEntity);


            // return the result
            return er;
        }

        /// <summary>
        /// Gets the corresponding event type for a specific method
        /// </summary>
        /// <param name="methodText">The method text from a VCalendar request</param>
        /// <returns>The event type corresponding with the text</returns>
        internal EventRequestType GetRequestTypeFromMethod(string methodText)
        {
            // See RFC2446 for a full list of methods
            switch (methodText.ToLowerInvariant())
            {
                case "request": 
                    return EventRequestType.Invite;

                case "cancel":
                    return EventRequestType.Cancel;

                default:
                    throw new VCalendarFormatException("Calendar method type '" + methodText + "' not supported.");
            }
        }

        public EventData GetEvent(VCardGroup group)
        {
            EventData e = new EventData();

            // Event Id
            VCardValue uid;
            if (!TryGetValue(group, "UID", out uid))
                throw new VCalendarFormatException("UID value not found");
            e.EventId = uid.GetText();

            // Start
            VCardValue start;
            if (!TryGetValue(group, "DTSTART", out start))
                throw new VCalendarFormatException("DTSTART value not found");
            e.Start = start.GetDateTime();

            // End
            VCardValue end;
            if (!TryGetValue(group, "DTEND", out end))
                throw new VCalendarFormatException("DTEND value not found");
            e.End = end.GetDateTime();

            // Organizer
            VCardValue organizer;
            if (!TryGetValue(group, "ORGANIZER", out organizer))
                throw new VCalendarFormatException("ORGANIZER value not found");
            e.Organizer = GetAttendee(organizer);

            // Summary
            VCardValue summary;
            if (TryGetValue(group, "SUMMARY", out summary))
                e.Summary = summary.GetText();
            else
            {
                e.Summary = "Event";
                if (!String.IsNullOrEmpty(e.Organizer.Name))
                    e.Summary += " by " + e.Organizer.Name;
            }

            // return the result
            return e;
        }

        public AttendeeData GetAttendee(VCardValue value)
        {
            AttendeeData a = new AttendeeData();

            // try to get the name
            VCardSimpleValue cn;
            if (value.Parameters.TryGetFirstEntity("CN", out cn))
            {
                a.Name = cn.GetText();

                if (a.Name.StartsWith("\"", StringComparison.InvariantCulture))
                {
                    a.Name = a.Name.Substring(1);
                    if (a.Name.EndsWith("\"", StringComparison.InvariantCulture))
                        a.Name = a.Name.Substring(0, a.Name.Length - 1);
                }
            }

            // parse the uri
            string valueText = value.GetText();
            if (!valueText.StartsWith("mailto:", StringComparison.InvariantCultureIgnoreCase))
                throw new VCalendarFormatException("The attendee value is not an email address in the URI format");
            a.Email = Uri.UnescapeDataString(valueText.Substring(7));

            // return the result
            return a;
        }

        private static bool TryGetValue(VCardGroup group, string name, out VCardValue value)
        {
            value = null;

            VCardEntity e;
            if (!group.Children.TryGetFirstEntity(name, out e))
                return false;

            if (!(e is VCardValue))
                return false;

            value = (VCardValue)e;
            return true;
        }
    }
}
