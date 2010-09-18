using System;
using ActiveUp.Net.Mail;

namespace Themis.Mail
{
    /// <summary>
    /// Wrapper for recieving messages using the MailSystem Pop3 implementation.
    /// </summary>
    public class MailSystemMailRetriever : IMailRetriever
    {
        public void GetMessages(MailboxConnectionInfo server, Func<IMailMessage, bool> messageRecievedCallback)
        {
            using (Pop3Client pop3 = new Pop3Client())
            {
                pop3.Connect(server.HostName, server.Port, server.Username, server.Password);

                int count = pop3.MessageCount;

                for (int i = 1; i <= count; i++)
                {
                    Message message = pop3.RetrieveMessageObject(i);

                    bool messageHandled = messageRecievedCallback(new MailSystemMailMessage(message));

                    if (messageHandled)
                        pop3.DeleteMessage(i);
                }

            }
        }
    }
}
