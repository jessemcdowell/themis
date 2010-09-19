using System;
using System.Text;
using Themis.Email;

namespace Themis.TestClient
{
    public class ReplyToAllEmails
    {
        private readonly IEmailRetriever _retriever;

        private readonly IEmailSender _sender;

        public ReplyToAllEmails(IEmailRetriever retriever, IEmailSender sender)
        {
            _retriever = retriever;
            _sender = sender;
        }

        public void Execute(MailboxConnectionInfo mailboxInfo)
        {
            Console.WriteLine("Replying to all email from {0}:{1}", mailboxInfo.ReceiveHostName, mailboxInfo.ReceivePort);

            _retriever.GetMessages(mailboxInfo, e => HandleMessage(e, mailboxInfo));

            Console.WriteLine("Done.");
        }

        private bool HandleMessage(IReceivedEmail email, MailboxConnectionInfo mailboxInfo)
        {
            Console.Write("- Sending email to {0}...", email.From.ToString());

            EmailBuilder response = BuildResponse(email);
            _sender.SendEmail(mailboxInfo, response);            

            Console.WriteLine(" Done");

            // pretend we failed so that we don't delete any emails
            return false;
        }

        private EmailBuilder BuildResponse(IReceivedEmail email)
        {
            EmailBuilder response = new EmailBuilder();
            response.To.Add(email.From);
            response.Subject = "Thank you for your email";

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Hello {0},\r\n", email.From.Name ?? email.From.Address);
            sb.AppendLine();
            sb.AppendLine("Thank you for your email.");
            sb.AppendLine();
            sb.AppendLine("This system is not yet fully operational, so please bear with us while we build support for processing your requests.");
            sb.AppendLine();
            sb.AppendLine("Thanks,");
            sb.AppendLine("Themis System.");

            response.TextBody = sb.ToString();

            return response;
        }
    }
}
