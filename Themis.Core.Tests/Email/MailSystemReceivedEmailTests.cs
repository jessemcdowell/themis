using System;
using NUnit.Framework;

namespace Themis.Email
{
    [TestFixture]
    public class MailSystemReceivedEmailTests
    {
        [Test]
        public void Wrap_Outlook_Email_1()
        {
            var rm = new MailSystemReceivedEmail(ExampleEmails.Messages.GetOutlook1NewEmail());

            Assert.AreEqual("Test Organizer", rm.From.Name, "From.Name");
            Assert.AreEqual("testorganizer@fake.foo", rm.From.Address, "From.Address");

            Assert.AreEqual("Test Appointment from Outlook", rm.Subject, "Subject");
        }

    }
}
