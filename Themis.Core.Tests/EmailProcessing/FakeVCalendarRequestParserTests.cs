using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using Themis.Calendar.VCard;

namespace Themis.EmailProcessing
{
    [TestFixture(Description = "Confirms that the FakeVCalendarRequestParser is working")]
    public class FakeVCalendarRequestParserTests
    {
        [Test]
        public void FakeVCalendarRequestParser_GetEventRequestFromVCard_Returns_Text_In_Event_Summary()
        {
            const string heading = "Heading";
            const string value = "value";
            const string expected = heading + ":" + value;

            var input = new VCardGroup("TestGroup");
            input.Children.Add(new VCardValue(heading, value));

            var actual = new FakeVCalendarRequestParser().GetEventRequestFromVCard(input);

            Assert.That(actual.Event.Summary, Contains.Substring(expected));
        }

        [Test]
        public void FakeVCalendarRequestParser_GetEventRequestFromVCardStream_Returns_Text_In_Event_Summary()
        {
            const string expected = "This is test data";
            var input = new StringReader(expected);

            var actual = new FakeVCalendarRequestParser().GetEventRequestFromVCardStream(input);

            Assert.That(actual.Event.Summary, Contains.Substring(expected));
        }

    }
}
