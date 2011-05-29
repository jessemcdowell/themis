using System;
using System.Collections.Generic;
using NUnit.Framework;
using Themis.Calendar;
using Themis.Calendar.VCard;
using Themis.ExampleEmails;
using Themis.Email;

namespace Themis.EmailProcessing
{
    [TestFixture]
    public class EmailCalendarRequestRetrieverTests
    {
        private const string CalendarMimeType = "text/calendar";

        private EmailCalendarRequestRetriever GetRetriever()
        {
            var retriever = new EmailCalendarRequestRetriever(new FakeVCalendarRequestParser());

            return retriever;
        }

        [Test]
        public void GetRequestFromEmail_Succeeds_For_Valid_Email()
        {
            IReceivedEmail input = Messages.GetRecievedEmail(Messages.GetOutlook1NewEmail());
            const string expectedContent = "UID:040000008200E00074C5B7101A82E0080000000030BE6F200884CB01";

            var retriever = GetRetriever();
            var actual = retriever.GetRequestFromEmail(input);

            Assert.IsNotNull(actual);
            Assert.That(actual.Event.Summary, Contains.Substring(expectedContent));
        }

        [Test]
        public void GetFirstCalendarSection_Finds_First_Section_In_First_Level()
        {
            var input = new IReceivedEmailSection[]
            {
                new FakeReceivedEmailSection() { ContentMimeType = CalendarMimeType},
                new FakeReceivedEmailSection(),
            };
            var expected = input[0];

            var actual = EmailCalendarRequestRetriever.GetFirstCalendarSection(input);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void GetFirstCalendarSection_Finds_Second_Section_In_First_Level()
        {
            var input = new IReceivedEmailSection[]
            {
                new FakeReceivedEmailSection(),
                new FakeReceivedEmailSection() { ContentMimeType = CalendarMimeType},
            };
            var expected = input[1];

            var actual = EmailCalendarRequestRetriever.GetFirstCalendarSection(input);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void GetFirstCalendarSection_Finds_First_Section_In_Second_Level()
        {
            var expected = new FakeReceivedEmailSection() { ContentMimeType = CalendarMimeType };

            var input = new IReceivedEmailSection[]
            {
                new FakeReceivedEmailSection(),
                new FakeReceivedEmailSection() { 
                    ChildSections = new List<IReceivedEmailSection>() 
                    {
                        expected,
                    },
                },
                
            };

            var actual = EmailCalendarRequestRetriever.GetFirstCalendarSection(input);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void GetFirstCalendarSection_Finds_First_Section_In_Second_Level_Before_Next_Item_In_Top_Level()
        {
            var expected = new FakeReceivedEmailSection() { ContentMimeType = CalendarMimeType };
            var input = new IReceivedEmailSection[]
            {
                new FakeReceivedEmailSection(),
                new FakeReceivedEmailSection() { 
                    ChildSections = new List<IReceivedEmailSection>() 
                    {
                        expected,
                        new FakeReceivedEmailSection(),
                    },
                },
                new FakeReceivedEmailSection() { ContentMimeType = CalendarMimeType},
            };

            var actual = EmailCalendarRequestRetriever.GetFirstCalendarSection(input);

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
