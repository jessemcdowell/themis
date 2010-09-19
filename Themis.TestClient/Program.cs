using System;
using System.Configuration;
using Castle.MicroKernel.Registration;
using Themis.Email;

namespace Themis.TestClient
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                RegisterTestClients();

                IEmailRetriever mailRetriever = Config.Container.Resolve<IEmailRetriever>();
                MailboxConnectionInfo mailboxInfo = GetMailboxInfoFromAppConfig();

                //Config.Container.Resolve<ListAllEmails>().Execute(mailboxInfo);
                Config.Container.Resolve<ReplyToAllEmails>().Execute(mailboxInfo);
                
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
                Username = ConfigurationManager.AppSettings["Username"],
                Password = ConfigurationManager.AppSettings["Password"],
                SmtpRequiresAuthentication = Boolean.Parse(ConfigurationManager.AppSettings["SmtpRequiresAuthentication"]),
            };

            mailboxInfo.EmailAddress = new EmailAddress(
                ConfigurationManager.AppSettings["EmailAddress"],
                ConfigurationManager.AppSettings["EmailName"]);


            // pop3 server
            string pop3Server = ConfigurationManager.AppSettings["Pop3Server"];
            if (String.IsNullOrEmpty(pop3Server))
                throw new ApplicationException("You must specify a POP3 server");

            string pop3Host;
            int pop3Port;
            ParseServerName(pop3Server, out pop3Host, out pop3Port, mailboxInfo.ReceivePort);
            mailboxInfo.ReceiveHostName = pop3Host;
            mailboxInfo.ReceivePort = pop3Port;


            // smtp server
            string smtpServer = ConfigurationManager.AppSettings["SmtpServer"];
            if (String.IsNullOrEmpty(smtpServer))
                throw new ApplicationException("You must specify an SMTP server");

            string smtpHost;
            int smtpPort;
            ParseServerName(smtpServer, out smtpHost, out smtpPort, mailboxInfo.SmtpPort);
            mailboxInfo.SmtpHostName = smtpHost;
            mailboxInfo.SmtpPort = smtpPort;


            return mailboxInfo;
        }

        private static void ParseServerName(string text, out string hostname, out int port, int defaultPort)
        {
            string[] hostParts = text.Split(':');

            // host name
            hostname = hostParts[0];

            // optional port
            if (hostParts.GetLength(0) > 1)
                port = Int32.Parse(hostParts[1]);
            else
                port = defaultPort;
        }

        private static void RegisterTestClients()
        {
            Config.Container.Register(
                AllTypes.FromAssembly(typeof(Program).Assembly).Where(t => true).Configure(c => c.LifeStyle.Transient)
                ); 
        }

        private static void Pause()
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
