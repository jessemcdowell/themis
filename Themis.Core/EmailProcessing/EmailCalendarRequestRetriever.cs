using System;
using System.Collections.Generic;
using Themis.Calendar;
using Themis.Email;

namespace Themis.EmailProcessing
{
    public class EmailCalendarRequestRetriever : IEmailCalendarRequestRetriever
    {
        private readonly IVCalendarRequestParser _vcalendarParser;

        private const string CalendarMimeType = "text/calendar";

        public EmailCalendarRequestRetriever(IVCalendarRequestParser vcalendarParser)
        {
            _vcalendarParser = vcalendarParser;
        }

        /// <summary>
        /// Finds, parses, and returns a VCalendar request from an email, or returns null if none is found.
        /// </summary>
        /// <param name="email">The email to search</param>
        /// <returns>The VCalendar request, or null if none is found</returns>
        public EventRequestData GetRequestFromEmail(IReceivedEmail email)
        {
            // look for the section
            IReceivedEmailSection section = GetFirstCalendarSection(email.Sections);
            if (section == null)
                return null;

            EventRequestData request;
            using (var reader = section.GetContentTextReader())
            {
                request = _vcalendarParser.GetEventRequestFromVCardStream(reader);
            }

            return request;
        }

        /// <summary>
        /// Recursively searches a list of sections for a child section that is a calendar request
        /// </summary>
        /// <param name="sectionList">The list of sections to search</param>
        /// <returns>The first child found, or null if none are found</returns>
        internal static IReceivedEmailSection GetFirstCalendarSection(IEnumerable<IReceivedEmailSection> sectionList)
        {
            foreach (IReceivedEmailSection section in sectionList)
            {
                // see if this note matches
                if (String.Equals(section.ContentMimeType, CalendarMimeType, StringComparison.InvariantCultureIgnoreCase))
                    return section;

                // search it's children
                var result = GetFirstCalendarSection(section.ChildSections);
                if (result != null)
                    return result;
            }

            return null;
        }
    }
}
