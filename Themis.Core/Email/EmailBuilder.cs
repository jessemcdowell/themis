using System;
using System.Collections.Generic;

namespace Themis.Email
{
    /// <summary>
    /// Builds and contains the contents of an email to send
    /// </summary>
    public class EmailBuilder
    {
        public EmailBuilder()
        {
            To = new List<EmailAddress>();
        }

        public ICollection<EmailAddress> To { get; private set; }

        public string Subject { get; set; }

        public string TextBody { get; set; }
    }
}
