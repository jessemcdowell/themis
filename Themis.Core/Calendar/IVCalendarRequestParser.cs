using System;
using Themis.Calendar.VCard;
using System.IO;

namespace Themis.Calendar
{
    public interface IVCalendarRequestParser
    {
        EventRequestData GetEventRequestFromVCard(VCardGroup document);

        EventRequestData GetEventRequestFromVCardStream(TextReader stream);
    }
}
