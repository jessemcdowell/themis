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


                Config.Container.Resolve<ListAllEmails>().Execute(mailboxInfo);
                
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
