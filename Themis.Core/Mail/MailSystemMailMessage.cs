using System;
using ActiveUp.Net.Mail;

namespace Themis.Mail
{
    /// <summary>
    /// Wrapper for a message returned from the MailSystem Pop3 implementation.
    /// </summary>
    public class MailSystemMailMessage : IMailMessage
    {
        private readonly Message _message;

        public MailSystemMailMessage(Message message)
        {
            _message = message;
        }

        public override string ToString()
        {
            return _message.Subject;
        }
    }
}
