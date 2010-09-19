using System;

namespace Themis.Email
{
    // interface for sending email
    public interface IEmailSender
    {
        void SendEmail(MailboxConnectionInfo mailbox, EmailBuilder message);
    }
}
