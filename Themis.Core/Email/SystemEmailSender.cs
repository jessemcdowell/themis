using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace Themis.Email
{
    public class SystemEmailSender : IEmailSender
    {
        public void SendEmail(MailboxConnectionInfo mailbox, EmailBuilder message)
        {
            using (SmtpClient client = new SmtpClient())
            {
                // configure the smtp client
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Host = mailbox.SmtpHostName;
                client.Port = mailbox.SmtpPort;
                
                if (mailbox.SmtpRequiresAuthentication)
                {
                    client.Credentials = new System.Net.NetworkCredential(mailbox.Username, mailbox.Password);
                }


                // send the email
                MailMessage mailMessage = ConvertEmail(message);

                mailMessage.From = ConvertEmailAddress(mailbox.EmailAddress);

                client.Send(mailMessage);
            }
        }

        internal MailMessage ConvertEmail(EmailBuilder source)
        {
            MailMessage msg = new MailMessage();
           
            AddEmailAdresses(source.To, msg.To);

            msg.Subject = source.Subject;

            msg.Body = source.TextBody;

            return msg;
        }

        internal void AddEmailAdresses(IEnumerable<EmailAddress> source, MailAddressCollection destination)
        {
            foreach (EmailAddress address in source)
                destination.Add(ConvertEmailAddress(address));
        }

        internal MailAddress ConvertEmailAddress(EmailAddress address)
        {
            return new MailAddress(address.Address, address.Name ?? String.Empty);
        }    

    }
}
