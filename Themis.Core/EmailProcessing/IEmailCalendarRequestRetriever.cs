using System;
using Themis.Calendar;
using Themis.Email;

namespace Themis.EmailProcessing
{
    public interface IEmailCalendarRequestRetriever
    {
        EventRequestData GetRequestFromEmail(IReceivedEmail email);
    }
}
