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

            Assert.AreEqual(1, rm.Sections.Count, "Section Count");
            Assert.That(rm.Sections[0].GetContentTextReader().ReadToEnd(), Contains.Substring("UID:040000008200E00074C5B7101A82E0080000000030BE6F200884CB01000000000000000"), "Content Contains"); 
        }

    }
}
