using System;

namespace Themis.Mail
{
    /// <summary>
    /// Information required for connecting to a mailbox.
    /// </summary>
    public class MailboxConnectionInfo
    {
        public const int DefaultPop3Port = 110;

        public MailboxConnectionInfo()
        {
            Port = DefaultPop3Port;
        }

        public string HostName { get; set; }

        public int Port { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

    }
}
