using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Themis.Email;
using Themis.EmailProcessing;

namespace Themis.TestClient
{
    public class WriteInvitationsToConsole
    {
        private readonly IEmailRetriever _retriever;
        private readonly IEmailCalendarRequestRetriever _requestRetriever;

        private int _index = 1;

        public WriteInvitationsToConsole(IEmailRetriever retriever, IEmailCalendarRequestRetriever requestRetriever)
        {
            _retriever = retriever;
            _requestRetriever = requestRetriever;
        }

        public void Execute(MailboxConnectionInfo mailboxInfo)
        {
            Console.WriteLine("Retrieving emails from {0} at {1}:{2}", mailboxInfo.Username, mailboxInfo.ReceiveHostName, mailboxInfo.ReceivePort);

            _retriever.GetMessages(mailboxInfo, HandleMessage);

            Console.WriteLine(); 
            Console.WriteLine("Done.");
        }

        private bool HandleMessage(IReceivedEmail email)
        {
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("Email {0}:", _index++);
            Console.WriteLine("  From: {0}", email.From);
            Console.WriteLine("  Subject: {0}", email.Subject);

            try
            {
                var request = _requestRetriever.GetRequestFromEmail(email);

                if (request == null)
                    Console.WriteLine("  VCalendar not found");
                else
                {

                    var e = request.Event;
                    Console.WriteLine("  Event ID: {0}", e.EventId);
                    Console.WriteLine("  Summary: {0}", e.Summary);
                    Console.WriteLine("  Time: {0} to {1}", e.Start, e.End);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("  Error retrieving calendar: {0}", ex.ToString());
            }
            
            // pretend we failed so that we don't delete any emails
            return false;
        }
    }
}
