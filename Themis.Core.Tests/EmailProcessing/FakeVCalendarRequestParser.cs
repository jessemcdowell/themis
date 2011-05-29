using System;
using System.Text;
using Themis.Calendar;
using Themis.Calendar.VCard;

namespace Themis.EmailProcessing
{
    /// <summary>
    /// Implementation of IVCalendarRequestParser that jams the entire content into the Summary field of the request data.
    /// </summary>
    internal class FakeVCalendarRequestParser : IVCalendarRequestParser
    {
        private EventRequestData CreateResult(string content)
        {
            return new EventRequestData()
            {
                Event = new EventData()
                {
                    Summary = content,
                },
            };
        }

        public EventRequestData GetEventRequestFromVCardStream(System.IO.TextReader stream)
        {
            return CreateResult(stream.ReadToEnd());
        }

        public EventRequestData GetEventRequestFromVCard(VCardGroup document)
        {
            StringBuilder output = new StringBuilder();
            WriteGroup(output, document);

            return CreateResult(output.ToString());
        }

        private void WriteGroup(StringBuilder output, VCardGroup group)
        {
            output.AppendFormat("BEGIN:{0}\r\n", group.Name);

            foreach (VCardEntity child in group.Children)
            {
                if (child is VCardGroup)
                {
                    WriteGroup(output, (VCardGroup)child);
                    continue;
                }
                else if (child is VCardValue)
                {
                    VCardValue value = (VCardValue)child;

                    output.Append(value.Name);

                    foreach (var p in value.Parameters)
                        output.AppendFormat(";{0}={1}", p.Name, p.EscapedValue);

                    output.AppendFormat(":{0}\r\n", value.EscapedValue);
                }
            }

            output.AppendFormat("END:{0}\r\n", group.Name);
        }

    }
}
