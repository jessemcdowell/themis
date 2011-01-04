using System;
using NUnit.Framework;
using Themis.Calendar.VCard;

namespace Themis.Calendar
{
    [TestFixture]
    public class VCalendarRequestParserTests
    {
        private static VCardEntity GetVCardEntity(string vcardLine)
        {
            VCardReader r = new VCardReader();
            using (var sr = new System.IO.StringReader(vcardLine))
            {
                return r.ReadEntity(sr);
            }
        }

        private VCalendarRequestParser GetVCalendarRequestParser()
        {
            return new VCalendarRequestParser();
        }

        [Test]
        public void GetRequestFromVCard_All_Values()
        {
            const string vcardInput =
@"BEGIN:VCALENDAR
VERSION:2.0
METHOD:REQUEST
BEGIN:VEVENT
UID:012345678901234567890
DTSTART:20101115T190000Z
DTEND:20101115T193000Z
ORGANIZER;CN=""Test Organizer"":mailto:testorganizer@fake.foo
SUMMARY:Simple Event
END:VEVENT
END:VCALENDAR
";
            VCardGroup input = (VCardGroup)GetVCardEntity(vcardInput);

            VCalendarRequestParser vcr = GetVCalendarRequestParser();
            EventRequestData actual = vcr.GetEventRequestFromVCard(input);

            Assert.IsNotNull(actual, "EventRequest");
            Assert.AreEqual(EventRequestType.Invite, actual.RequestType, "RequestType");

            Assert.IsNotNull(actual.Event, "Event");
            Assert.AreEqual("012345678901234567890", actual.Event.EventId, "Event.EventId");
        }

        [Test]
        [ExpectedException(typeof(VCalendarFormatException), ExpectedMessage = "VCalendar version not supported")]
        public void GetRequestFromVCard_With_Wrong_Version_Fails()
        {
            const string vcardInput =
@"BEGIN:VCALENDAR
VERSION:3.0
METHOD:REQUEST
BEGIN:VEVENT
UID:012345678901234567890
DTSTART:20101115T190000Z
DTEND:20101115T193000Z
ORGANIZER;CN=""Test Organizer"":mailto:testorganizer@fake.foo
SUMMARY:Simple Event
END:VEVENT
END:VCALENDAR
";
            VCardGroup input = (VCardGroup)GetVCardEntity(vcardInput);

            VCalendarRequestParser vcr = GetVCalendarRequestParser();
            
            vcr.GetEventRequestFromVCard(input);
        }

        [Test]
        [ExpectedException(typeof(VCalendarFormatException), ExpectedMessage = "METHOD value not found")]
        public void GetRequestFromVCard_With_No_Method_Fails()
        {
            const string vcardInput =
@"BEGIN:VCALENDAR
VERSION:2.0
BEGIN:VEVENT
UID:012345678901234567890
DTSTART:20101115T190000Z
DTEND:20101115T193000Z
ORGANIZER;CN=""Test Organizer"":mailto:testorganizer@fake.foo
SUMMARY:Simple Event
END:VEVENT
END:VCALENDAR
";
            VCardGroup input = (VCardGroup)GetVCardEntity(vcardInput);

            VCalendarRequestParser vcr = GetVCalendarRequestParser();

            vcr.GetEventRequestFromVCard(input);
        }

        [Test]
        [ExpectedException(typeof(VCalendarFormatException), ExpectedMessage = "VEVENT group not found")]
        public void GetRequestFromVCard_With_No_Event_Fails()
        {
            const string vcardInput =
@"BEGIN:VCALENDAR
VERSION:2.0
METHOD:REQUEST
END:VCALENDAR
";
            VCardGroup input = (VCardGroup)GetVCardEntity(vcardInput);

            VCalendarRequestParser vcr = GetVCalendarRequestParser();

            vcr.GetEventRequestFromVCard(input);
        }

        [Test]
        [ExpectedException(typeof(VCalendarFormatException), ExpectedMessage = "VEVENT group not found")]
        public void GetRequestFromVCard_With_Event_As_Value_Fails()
        {
            const string vcardInput =
@"BEGIN:VCALENDAR
VERSION:2.0
METHOD:REQUEST
VEVENT:hahaha
END:VCALENDAR
";
            VCardGroup input = (VCardGroup)GetVCardEntity(vcardInput);

            VCalendarRequestParser vcr = GetVCalendarRequestParser();

            vcr.GetEventRequestFromVCard(input);
        }

        [Test]
        public void GetEvent_All_Values()
        {
            const string vcardInput =
@"BEGIN:VEVENT
UID:012345678901234567890
DTSTART:20101115T190000Z
DTEND:20101115T193000Z
ORGANIZER;CN=""Test Organizer"":mailto:testorganizer@fake.foo
SUMMARY:Simple Event
END:VEVENT
";
            VCardGroup input = (VCardGroup)GetVCardEntity(vcardInput);

            VCalendarRequestParser vcr = GetVCalendarRequestParser();
            EventData actual = vcr.GetEvent(input);

            Assert.IsNotNull(actual, "Event");
            Assert.AreEqual("012345678901234567890", actual.EventId, "EventID");
            DateTimeAssert.AreEqual(new DateTime(2010, 11, 15, 19, 00, 00, DateTimeKind.Utc), actual.Start);
            DateTimeAssert.AreEqual(new DateTime(2010, 11, 15, 19, 30, 00, DateTimeKind.Utc), actual.End);
            Assert.IsNotNull(actual.Organizer, "Event.Organizer");
            Assert.AreEqual("Test Organizer", actual.Organizer.Name, "Organizer Name");
            Assert.AreEqual("testorganizer@fake.foo", actual.Organizer.Email, "Organizer Email");
            Assert.AreEqual("Simple Event", actual.Summary);
        }

        [Test]
        [ExpectedException(typeof(VCalendarFormatException), ExpectedMessage = "UID value not found")]
        public void GetEvent_Without_EventId_Fails()
        {
            const string vcardInput =
@"BEGIN:VEVENT
DTSTART:20101115T190000Z
DTEND:20101115T193000Z
ORGANIZER;CN=""Test Organizer"":mailto:testorganizer@fake.foo
SUMMARY:Simple Event
END:VEVENT
";
            VCardGroup input = (VCardGroup)GetVCardEntity(vcardInput);

            VCalendarRequestParser vcr = GetVCalendarRequestParser();
            
            vcr.GetEvent(input);
        }

        [Test]
        [ExpectedException(typeof(VCalendarFormatException), ExpectedMessage = "DTSTART value not found")]
        public void GetEvent_Without_Start_Fails()
        {
            const string vcardInput =
@"BEGIN:VEVENT
UID:012345678901234567890
DTEND:20101115T193000Z
ORGANIZER;CN=""Test Organizer"":mailto:testorganizer@fake.foo
SUMMARY:Simple Event
END:VEVENT
";
            VCardGroup input = (VCardGroup)GetVCardEntity(vcardInput);

            VCalendarRequestParser vcr = GetVCalendarRequestParser();

            vcr.GetEvent(input);
        }

        [Test]
        [ExpectedException(typeof(VCalendarFormatException), ExpectedMessage = "DTEND value not found")]
        public void GetEvent_Without_End_Fails()
        {
            const string vcardInput =
@"BEGIN:VEVENT
UID:012345678901234567890
DTSTART:20101115T190000Z
ORGANIZER;CN=""Test Organizer"":mailto:testorganizer@fake.foo
SUMMARY:Simple Event
END:VEVENT
";
            VCardGroup input = (VCardGroup)GetVCardEntity(vcardInput);

            VCalendarRequestParser vcr = GetVCalendarRequestParser();

            vcr.GetEvent(input);
        }

        [Test]
        [ExpectedException(typeof(VCalendarFormatException), ExpectedMessage = "ORGANIZER value not found")]
        public void GetEvent_Without_Organizer_Fails()
        {
            const string vcardInput =
@"BEGIN:VEVENT
UID:012345678901234567890
DTSTART:20101115T190000Z
DTEND:20101115T193000Z
SUMMARY:Simple Event
END:VEVENT
";
            VCardGroup input = (VCardGroup)GetVCardEntity(vcardInput);

            VCalendarRequestParser vcr = GetVCalendarRequestParser();

            vcr.GetEvent(input);
        }

        [Test]
        public void GetEvent_Without_Summary_But_Has_Organizer_Name_Gets_Summary_With_Organizer_Name()
        {
            const string vcardInput =
@"BEGIN:VEVENT
UID:012345678901234567890
DTSTART:20101115T190000Z
DTEND:20101115T193000Z
ORGANIZER;CN=""Test Organizer"":mailto:testorganizer@fake.foo
END:VEVENT
";
            VCardGroup input = (VCardGroup)GetVCardEntity(vcardInput);

            VCalendarRequestParser vcr = GetVCalendarRequestParser();
            EventData actual = vcr.GetEvent(input);

            Assert.AreEqual("Event by Test Organizer", actual.Summary);
        }

        [Test]
        public void GetEvent_Without_Summary_Or_Organizer_Name_Gets_Generic_Summary()
        {
            const string vcardInput =
@"BEGIN:VEVENT
UID:012345678901234567890
DTSTART:20101115T190000Z
DTEND:20101115T193000Z
ORGANIZER:mailto:testorganizer@fake.foo
END:VEVENT
";
            VCardGroup input = (VCardGroup)GetVCardEntity(vcardInput);

            VCalendarRequestParser vcr = GetVCalendarRequestParser();
            EventData actual = vcr.GetEvent(input);

            Assert.AreEqual("Event", actual.Summary);
        }

        [Test]
        public void GetAttendee_From_Outlook()
        {
            const string inputLine = "ATTENDEE;CN=Test User;RSVP=TRUE:mailto:testuser@fake.foo";
            var input = (VCardValue)GetVCardEntity(inputLine);

            VCalendarRequestParser vcr = GetVCalendarRequestParser();

            AttendeeData actual = vcr.GetAttendee(input);

            Assert.AreEqual("Test User", actual.Name, "Name");
            Assert.AreEqual("testuser@fake.foo", actual.Email, "Email");
        }

        [Test]
        public void GetAttendee_Without_Name()
        {
            const string inputLine = "ATTENDEE;RSVP=TRUE:mailto:testuser@fake.foo";
            var input = (VCardValue)GetVCardEntity(inputLine);

            VCalendarRequestParser vcr = GetVCalendarRequestParser();

            AttendeeData actual = vcr.GetAttendee(input);

            Assert.IsNullOrEmpty(actual.Name, "Name");
            Assert.AreEqual("testuser@fake.foo", actual.Email, "Email");
        }

        [Test]
        public void GetAttendee_Name_With_Quotes_Has_Quotes_Stripped()
        {
            const string inputLine = @"ORGANIZER;CN=""Test Organizer"":mailto:testorganizer@fake.foo";
            var input = (VCardValue)GetVCardEntity(inputLine);

            VCalendarRequestParser vcr = GetVCalendarRequestParser();

            AttendeeData actual = vcr.GetAttendee(input);

            Assert.AreEqual("Test Organizer", actual.Name, "Name");
            Assert.AreEqual("testorganizer@fake.foo", actual.Email, "Email");
        }

        [Test]
        [ExpectedException(typeof(VCalendarFormatException), ExpectedMessage = "The attendee value is not an email address in the URI format")]
        public void GetAttendee_With_Non_Mail_Uri_Value_Fails()
        {
            const string inputLine = "ATTENDEE:testuser@fake.foo";
            var input = (VCardValue)GetVCardEntity(inputLine);

            VCalendarRequestParser vcr = GetVCalendarRequestParser();

            vcr.GetAttendee(input);
        }

        [Test]
        [TestCase("REQUEST", EventRequestType.Invite)]
        [TestCase("request", EventRequestType.Invite)]
        [TestCase("rEqUeSt", EventRequestType.Invite)]
        [TestCase("CANCEL", EventRequestType.Cancel)]
        public void GetRequestTypeFromMethod_Known_Types(string input, EventRequestType expected)
        {
            VCalendarRequestParser vcr = GetVCalendarRequestParser();
            var actual = vcr.GetRequestTypeFromMethod(input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [ExpectedException(typeof(VCalendarFormatException))]
        [TestCase("ADD")]
        [TestCase("ROTATE")]
        public void GetRequestTypeFromMethod_Unknown_Known_Type_Fails(string input)
        {
            VCalendarRequestParser vcr = GetVCalendarRequestParser();
            
            vcr.GetRequestTypeFromMethod(input);
        }
    }
}
