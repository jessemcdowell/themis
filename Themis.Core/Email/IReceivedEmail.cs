using System;
using System.Collections.Generic;

namespace Themis.Email
{
    public interface IReceivedEmail
    {
        EmailAddress From { get; }

        string Subject { get; }

        IList<IReceivedEmailSection> Sections { get; }
    }
}
