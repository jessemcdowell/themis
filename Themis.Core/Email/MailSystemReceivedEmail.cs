using System;
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
    }
}
