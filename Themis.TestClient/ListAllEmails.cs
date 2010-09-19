using System;
using Themis.Email;

namespace Themis.TestClient
{
    public class ListAllEmails
    {
        private readonly IEmailRetriever _retriever;

        public ListAllEmails(IEmailRetriever retriever)
        {
            _retriever = retriever;
        }

        public void Execute(MailboxConnectionInfo mailboxInfo)
        {
            Console.WriteLine("Retrieving email from {0}:{1}", mailboxInfo.ReceiveHostName, mailboxInfo.ReceivePort);

            _retriever.GetMessages(mailboxInfo, message =>
            {
                Console.WriteLine("- From: {0}, Subject: {1}", message.From.ToString(), message.Subject);
                return false;
            });

            Console.WriteLine("Done.");
        }
    }
}
