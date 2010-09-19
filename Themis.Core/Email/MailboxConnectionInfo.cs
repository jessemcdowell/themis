using System;

namespace Themis.Email
{
    /// <summary>
    /// Information required for connecting to a mailbox.
    /// </summary>
    public class MailboxConnectionInfo
    {
        public const int DefaultPop3Port = 110;
        public const int DefaultSmtp3Port = 25;

        public MailboxConnectionInfo()
        {
            ReceivePort = DefaultPop3Port;
            SmtpPort = DefaultSmtp3Port;
        }

        public string ReceiveHostName { get; set; }

        public int ReceivePort { get; set; }

        public string SmtpHostName { get; set; }

        public int SmtpPort { get; set; }

        public EmailAddress EmailAddress { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public bool SmtpRequiresAuthentication { get; set; }
    }
}
