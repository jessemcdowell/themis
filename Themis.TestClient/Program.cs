using System;
using System.Configuration;
using Themis.Email;

namespace Themis.TestClient
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                IEmailRetriever mailRetriever = Config.Container.Resolve<IEmailRetriever>();

                MailboxConnectionInfo mailboxInfo = GetMailboxInfoFromAppConfig();

                Console.WriteLine("Retrieving email from {0}:{1}", mailboxInfo.HostName, mailboxInfo.Port);

                mailRetriever.GetMessages(mailboxInfo, HandleMessage);

                Console.WriteLine("Done.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.ToString());
            }
            Pause();
        }

        private static MailboxConnectionInfo GetMailboxInfoFromAppConfig()
        {
            MailboxConnectionInfo mailboxInfo = new MailboxConnectionInfo()
            {
                Username = ConfigurationManager.AppSettings["MailboxUsername"],
                Password = ConfigurationManager.AppSettings["MailboxPassword"],
            };

            // get the hostname with optional :port
            string hostNameText = ConfigurationManager.AppSettings["MailboxHostname"];
            if (String.IsNullOrEmpty(hostNameText))
                throw new ApplicationException("You must configure the hostname, username, and password in the app config file");

            string[] hostParts = hostNameText.Split(':');

            mailboxInfo.HostName = hostParts[0];

            if (hostParts.GetLength(0) > 1)
                mailboxInfo.Port = Int32.Parse(hostParts[1]);

            return mailboxInfo;
        }

        private static bool HandleMessage(IReceivedEmail message)
        {
            Console.WriteLine("- {0}", message.ToString());
            return false;
        }


        private static void Pause()
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
