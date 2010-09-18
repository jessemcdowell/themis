using System;
using Castle.Windsor;
using Castle.MicroKernel.Registration;

namespace Themis
{
    public static class Config
    {
        /// <summary>
        /// Main IoC container for the system.
        /// </summary>
        public static IWindsorContainer Container { get; private set; }

        static Config()
        {
            Container = new WindsorContainer();
            SetupContainer();
        }

        private static void SetupContainer()
        {
            Container.Register(

                Component.For<Email.IEmailRetriever>().ImplementedBy<Email.MailSystemEmailRetriever>()

                );
        }

    }
}
