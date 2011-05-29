using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ActiveUp.Net.Mail;

namespace Themis.Email
{
    /// <summary>
    /// Wrapper for a message returned from the MailSystem Pop3 implementation.
    /// </summary>
    public class MailSystemReceivedEmail : IReceivedEmail
    {
        private readonly Message _message;

        public MailSystemReceivedEmail(Message message)
        {
            _message = message;

            PopulateSections();
        }

        public override string ToString()
        {
            return _message.Subject;
        }

        public EmailAddress From
        {
            get { return new EmailAddress(_message.From.Email, _message.From.Name); }
        }

        public string Subject
        {
            get { return _message.Subject; }
        }

        public IList<IReceivedEmailSection> Sections { get; private set; }

        private void PopulateSections()
        {
            List<IReceivedEmailSection> sections = new List<IReceivedEmailSection>();

            // the first section is the email itself
            var topSection = new ReceivedSection(_message);
            sections.Add(topSection);

            Sections = sections.AsReadOnly();
        }

        private class ReceivedSection : IReceivedEmailSection
        {
            private readonly Message _message;
           
            public ReceivedSection(Message message)
            {
                _message = message;
                PopulateSections();
            }

            public IList<IReceivedEmailSection> ChildSections { get; private set; }

            private void PopulateSections()
            {
                List<IReceivedEmailSection> sections = new List<IReceivedEmailSection>();

                foreach (Message child in _message.SubMessages)
                    sections.Add(new ReceivedSection(child));

                ChildSections = sections.AsReadOnly();
            }

            public string ContentMimeType
            {
                get { return _message.ContentType.MimeType; }
            }

            public TextReader GetContentTextReader()
            {
                return new StringReader(_message.LeafMimeParts[0].TextContent); 
            }
        }
    }
}
